using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Publishers
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
        public Publisher Publisher { get; set; }

        public IActionResult OnGet(int id)
        {
            // Lấy dữ liệu nhà xuất bản từ cơ sở dữ liệu
            Publisher = _context.Publishers.FirstOrDefault(p => p.PublisherId == id);

            if (Publisher == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var publisherInDb = _context.Publishers.FirstOrDefault(p => p.PublisherId == id);
            if (publisherInDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin nhà xuất bản
            publisherInDb.Name = Publisher.Name;
            publisherInDb.Address = Publisher.Address;
            publisherInDb.Contact = Publisher.Contact;

            try
            {
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Publisher updated successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, "An error occurred while updating the publisher.");
                return Page();
            }
        }
    }
}
