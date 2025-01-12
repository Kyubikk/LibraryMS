using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Categories
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
        public Category Category { get; set; }

        public IActionResult OnGet(int id)
        {
            // Lấy danh mục từ database theo ID
            Category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

            if (Category == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            var categoryInDb = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (categoryInDb == null)
            {
                return NotFound();
            }

            // Xóa danh mục
            _context.Categories.Remove(categoryInDb);

            // Lưu thay đổi vào database
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
