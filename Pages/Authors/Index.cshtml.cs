using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace LoginSystemApp.Pages.Authors
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        // Danh sách các Authors và chuỗi tìm kiếm
        public PaginatedList<Author> Authors { get; set; }
        public string SearchString { get; set; }

        public void OnGet(string? searchString, int? pageIndex)
        {
            // Gán chuỗi tìm kiếm
            SearchString = searchString ?? string.Empty;

            // Truy vấn từ cơ sở dữ liệu
            var query = _context.Authors.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(a => 
                    (!string.IsNullOrEmpty(a.Name) && a.Name.Contains(SearchString)) ||
                    (!string.IsNullOrEmpty(a.Bio) && a.Bio.Contains(SearchString)));
            }

            // Phân trang
            int pageSize = 5; // Hiển thị 5 dòng mỗi trang
            Authors = PaginatedList<Author>.Create(query, pageIndex ?? 1, pageSize);
        }
    }
}
