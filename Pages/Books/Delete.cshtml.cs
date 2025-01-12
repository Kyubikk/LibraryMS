using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Books
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly LibraryContext _context;

        public DeleteModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; }

        public IActionResult OnGet(int id)
        {
            Book = _context.Books.Find(id);
            if (Book == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var bookInDb = _context.Books.Find(id);
            if (bookInDb == null)
            {
                return NotFound();
            }

            _context.Books.Remove(bookInDb);
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
