using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Categories
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
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var categoryInDb = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (categoryInDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin danh mục
            categoryInDb.CategoryName = Category.CategoryName;

            // Lưu thay đổi vào database
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
