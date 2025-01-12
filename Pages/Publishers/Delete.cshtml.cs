using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Publishers
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
        public Publisher Publisher { get; set; }

        public IActionResult OnGet(int id)
        {
            // Tìm nhà xuất bản trong cơ sở dữ liệu
            Publisher = _context.Publishers.FirstOrDefault(p => p.PublisherId == id);

            if (Publisher == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            // Tìm nhà xuất bản cần xóa
            var publisherInDb = _context.Publishers.FirstOrDefault(p => p.PublisherId == id);
            if (publisherInDb == null)
            {
                return NotFound();
            }

            try
            {
                // Xóa nhà xuất bản
                _context.Publishers.Remove(publisherInDb);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Publisher deleted successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the publisher.");
                return Page();
            }
        }
    }
}
