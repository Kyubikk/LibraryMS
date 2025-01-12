using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Categories
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public List<Category> Categories { get; set; }

        public void OnGet()
        {
            // Lấy tất cả các danh mục từ cơ sở dữ liệu
            Categories = _context.Categories.OrderBy(c => c.CategoryName).ToList();
        }
    }
}
