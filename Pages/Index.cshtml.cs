using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace LoginSystemApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public IndexModel(LibraryContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public PaginatedList<Book> Books { get; set; }
        public string SearchString { get; set; }

        public void OnGet(string? searchString, int? pageIndex)
        {
            SearchString = searchString ?? string.Empty;

            // Lấy danh sách sách từ cơ sở dữ liệu với Include để lấy dữ liệu liên quan (Author, Publisher, Category)
            var query = _context.Books
                                .Include(b => b.Author)     // Bao gồm Author
                                .Include(b => b.Publisher)  // Bao gồm Publisher
                                .Include(b => b.Category)   // Bao gồm Category
                                .AsQueryable();

            // Nếu có tìm kiếm, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(b =>
                    b.Title.Contains(SearchString) ||
                    (b.Author != null && b.Author.Name.Contains(SearchString)) ||
                    (b.Publisher != null && b.Publisher.Name.Contains(SearchString)) ||
                    (b.Category != null && b.Category.CategoryName.Contains(SearchString)));
            }

            // Cài đặt phân trang
            int pageSize = 6;
            Books = PaginatedList<Book>.Create(query, pageIndex ?? 1, pageSize);
        }
    }
}
