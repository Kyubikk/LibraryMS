using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using Microsoft.AspNetCore.Authorization;
using LoginSystemApp.Models;

namespace LoginSystemApp.Pages.Authors
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
        public Author Author { get; set; }

        public IActionResult OnGet(int id)
        {
            // Tìm tác giả trong cơ sở dữ liệu theo id
            Author = _context.Authors.Find(id);
            if (Author == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            // Tìm tác giả trong cơ sở dữ liệu
            var authorInDb = _context.Authors.Find(id);
            if (authorInDb == null)
            {
                return NotFound();
            }

            // Xóa tác giả khỏi cơ sở dữ liệu
            _context.Authors.Remove(authorInDb);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Author deleted successfully!";
            return RedirectToPage("./Index");
        }
    }
}
