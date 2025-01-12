using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.Publishers
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public PaginatedList<Publisher> Publishers { get; set; }
        public string SearchString { get; set; }

        public void OnGet(string? searchString, int? pageIndex)
        {
            SearchString = searchString ?? string.Empty;

            var query = _context.Publishers.AsQueryable();

            if (!string.IsNullOrEmpty(SearchString))
            {
                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.Name) && p.Name.Contains(SearchString)) ||
                    (!string.IsNullOrEmpty(p.Address) && p.Address.Contains(SearchString)) ||
                    (!string.IsNullOrEmpty(p.Contact) && p.Contact.Contains(SearchString)));
            }

            int pageSize = 5;
            Publishers = PaginatedList<Publisher>.Create(query, pageIndex ?? 1, pageSize);
        }
    }
}
