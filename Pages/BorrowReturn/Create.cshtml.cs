using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.BorrowReturn
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly LibraryContext _context;

        public CreateModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public BorrowReturnTransaction BorrowReturn { get; set; } = new BorrowReturnTransaction();

        [BindProperty]
        public Dictionary<int, int> BookQuantities { get; set; } = new Dictionary<int, int>();

        public List<Book> AvailableBooks { get; set; } = new List<Book>();

        public async Task<IActionResult> OnGetAsync()
        {
            AvailableBooks = await _context.Books
                .Where(b => b.Available > 0)
                .ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
{
    // Xóa kiểm tra thuộc tính không cần thiết
    ModelState.Remove("BorrowReturn.BorrowReturnDetails");

    if (!ModelState.IsValid)
    {
        return Page();
    }

    if (BookQuantities == null || !BookQuantities.Any())
    {
        ModelState.AddModelError("", "Please select at least one book.");
        return Page();
    }

    var executionStrategy = _context.Database.CreateExecutionStrategy();
    IActionResult result = Page(); // Biến tạm để lưu trữ kết quả

    await executionStrategy.ExecuteAsync(async () =>
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            BorrowReturn.Status ??= "Borrowed"; // Giá trị mặc định
            BorrowReturn.Fine ??= 0; // Giá trị mặc định

            await _context.BorrowReturns.AddAsync(BorrowReturn);
            await _context.SaveChangesAsync();

            foreach (var bookEntry in BookQuantities)
            {
                var book = await _context.Books.FindAsync(bookEntry.Key);
                if (book == null || book.Available < bookEntry.Value)
                {
                    throw new Exception($"Invalid book selection or insufficient stock for Book ID: {bookEntry.Key}");
                }

                var borrowDetail = new BorrowReturnDetail
                {
                    TransactionId = BorrowReturn.TransactionId,
                    BookId = bookEntry.Key,
                    Quantity = bookEntry.Value
                };

                _context.BorrowReturnDetails.Add(borrowDetail);
                _context.Books.Update(book);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            result = RedirectToPage("./Index"); // Gán kết quả sau khi hoàn thành giao dịch
        }
        catch
        {
            await transaction.RollbackAsync();
            ModelState.AddModelError("", "Failed to save the transaction.");
            result = Page(); // Gán kết quả nếu có lỗi
        }
    });

    return result; // Trả về kết quả
}

        
    }
}
