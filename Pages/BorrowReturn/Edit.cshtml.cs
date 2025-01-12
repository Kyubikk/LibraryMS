using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.BorrowReturn
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly LibraryContext _context;

        public EditModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BorrowReturnTransaction BorrowReturn { get; set; } = new BorrowReturnTransaction();

        [BindProperty]
        public Dictionary<int, int> BookQuantities { get; set; } = new Dictionary<int, int>();

        public List<Book> AvailableBooks { get; set; } = new List<Book>();
        public List<BorrowReturnDetail> BorrowedBooks { get; set; } = new List<BorrowReturnDetail>();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            BorrowReturn = await _context.BorrowReturns
                .Include(br => br.BorrowReturnDetails)
                .ThenInclude(detail => detail.Book)
                .FirstOrDefaultAsync(br => br.TransactionId == id);

            if (BorrowReturn == null)
            {
                return NotFound();
            }

            BorrowedBooks = BorrowReturn.BorrowReturnDetails.ToList();
            AvailableBooks = await _context.Books
                .Where(b => b.Available > 0)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var transaction = await _context.BorrowReturns
                .Include(br => br.BorrowReturnDetails)
                .ThenInclude(detail => detail.Book)
                .FirstOrDefaultAsync(br => br.TransactionId == BorrowReturn.TransactionId);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.BorrowerName = BorrowReturn.BorrowerName;
            transaction.BorrowerContact = BorrowReturn.BorrowerContact;
            transaction.DueDate = BorrowReturn.DueDate;
            transaction.Fine = BorrowReturn.Fine;

            // Update Borrowed Books
            foreach (var detail in transaction.BorrowReturnDetails)
            {
                var book = await _context.Books.FindAsync(detail.BookId);
                if (book != null)
                {
                    book.Available += detail.Quantity; 
                }
            }
            transaction.BorrowReturnDetails.Clear();

            if (BookQuantities != null)
            {
                foreach (var bookEntry in BookQuantities)
                {
                    var book = await _context.Books.FindAsync(bookEntry.Key);
                    if (book == null || book.Available < bookEntry.Value)
                    {
                        ModelState.AddModelError("", $"Invalid book selection or insufficient stock for Book ID: {bookEntry.Key}");
                        return Page();
                    }

                    transaction.BorrowReturnDetails.Add(new BorrowReturnDetail
                    {
                        TransactionId = transaction.TransactionId,
                        BookId = bookEntry.Key,
                        Quantity = bookEntry.Value
                    });

                    book.Available -= bookEntry.Value;
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }
    }
}
