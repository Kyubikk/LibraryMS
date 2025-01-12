using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Localization; 
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace LoginSystemApp.Pages.Books
{
    [Authorize] 
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
            // Gán giá trị tìm kiếm
            SearchString = searchString ?? string.Empty;

            // Lấy danh sách sách từ cơ sở dữ liệu
            var query = _context.Books
                                .Include(b => b.Author)
                                .Include(b => b.Publisher)
                                .Include(b => b.Category)
                                .AsQueryable();

            // Nếu có tìm kiếm, áp dụng bộ lọc
            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(b =>
                    (!string.IsNullOrEmpty(b.Title) && b.Title.Contains(SearchString)) ||
                    (b.Author != null && !string.IsNullOrEmpty(b.Author.Name) && b.Author.Name.Contains(SearchString)) ||
                    (b.Publisher != null && !string.IsNullOrEmpty(b.Publisher.Name) && b.Publisher.Name.Contains(SearchString)) ||
                    (b.Category != null && !string.IsNullOrEmpty(b.Category.CategoryName) && b.Category.CategoryName.Contains(SearchString)));
            }

            // Áp dụng phân trang
            int pageSize = 5;
            Books = PaginatedList<Book>.Create(query, pageIndex ?? 1, pageSize);
        }

        public JsonResult OnGetDetails(int bookId)
        {
            var book = _context.Books
                .Include(b => b.Author)
                .Include(b => b.Publisher)
                .Include(b => b.Category)
                .FirstOrDefault(b => b.BookId == bookId);

            if (book == null)
            {
                return new JsonResult(new { success = false, message = _localizer["BookNotFound"] });
            }

            return new JsonResult(new
            {
                success = true,
                title = book.Title ?? _localizer["NotAvailable"],
                authorName = book.Author?.Name ?? _localizer["NotAvailable"],
                publisherName = book.Publisher?.Name ?? _localizer["NotAvailable"],
                yearPublished = book.YearPublished > 0 ? book.YearPublished.ToString() : _localizer["NotAvailable"],
                isbn = !string.IsNullOrEmpty(book.ISBN) ? book.ISBN : _localizer["NotAvailable"],
                categoryName = book.Category?.CategoryName ?? _localizer["NotAvailable"],
                available = book.Available > 0 ? book.Available.ToString() : _localizer["NotAvailable"],
                coverImagePath = !string.IsNullOrEmpty(book.CoverImagePath) ? book.CoverImagePath : "default-image.jpg"
            });
        }
    }
}
