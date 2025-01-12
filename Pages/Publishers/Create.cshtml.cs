using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Publishers
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
        public Publisher Publisher { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                // Thêm publisher mới vào cơ sở dữ liệu
                _context.Publishers.Add(Publisher);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Publisher added successfully!";
                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, "An error occurred while adding the publisher.");
                return Page();
            }
        }
    }
}
