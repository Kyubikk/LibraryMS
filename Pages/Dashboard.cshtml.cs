using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using System.Linq;

namespace LoginSystemApp.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly LibraryContext _context;

        public DashboardModel(LibraryContext context)
        {
            _context = context;
        }

        // Properties to store data for Dashboard
        public int TotalBooks { get; set; }
        public int AvailableBooks { get; set; }
        public int BooksBorrowed { get; set; }
        public int OverdueTransactions { get; set; }
        public int LowStockBooks { get; set; }
        public List<string> CategoryLabels { get; set; }
        public List<int> CategoryData { get; set; }
        public List<string> AuthorLabels { get; set; }
        public List<int> AuthorData { get; set; }
        public List<string> TransactionDates { get; set; }
        public List<int> TransactionData { get; set; }

        // Top 5 Books Borrowed
        public List<BookInfo> TopBooks { get; set; }

        public class BookInfo
        {
            public string Title { get; set; }
            public int BorrowCount { get; set; }
        }

        // OnGet method that retrieves all the required data
        public void OnGet()
        {
            // Retrieve total number of books in the library
            TotalBooks = _context.Books.Sum(b => b.Quantity);

            // Retrieve number of available books
            AvailableBooks = _context.Books.Sum(b => b.Available);

            // Retrieve number of borrowed books
            BooksBorrowed = _context.BorrowReturns
                .Where(bt => bt.Status == "Borrowed")
                .Sum(bt => bt.BorrowReturnDetails.Sum(bd => bd.Quantity));

            // Retrieve overdue transactions
            OverdueTransactions = _context.BorrowReturns
                .Where(bt => bt.Status == "Overdue")
                .Count();

            // Retrieve low stock books (less than 5 in stock)
            LowStockBooks = _context.Books
                .Where(b => b.Available < 5)
                .Count();

            // Get books by category
            var categoryData = _context.Categories
                .Select(c => new
                {
                    Category = c.CategoryName,
                    Count = c.Books.Count()
                }).ToList();

            CategoryLabels = categoryData.Select(c => c.Category).ToList();
            CategoryData = categoryData.Select(c => c.Count).ToList();

            // Get books by author
            var authorData = _context.Authors
                .Select(a => new
                {
                    Author = a.Name,
                    Count = a.Books.Count()
                })
                .OrderByDescending(a => a.Count)
                .Take(5)
                .ToList();

            AuthorLabels = authorData.Select(a => a.Author).ToList();
            AuthorData = authorData.Select(a => a.Count).ToList();

            // Get transactions over time
            var transactionsData = _context.BorrowReturns
                .GroupBy(bt => bt.BorrowDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                }).ToList();

            TransactionDates = transactionsData.Select(t => $"Month {t.Month}").ToList();
            TransactionData = transactionsData.Select(t => t.Count).ToList();

            // Get top 5 most borrowed books
            TopBooks = _context.Books
                .Select(b => new BookInfo
                {
                    Title = b.Title,
                    BorrowCount = _context.BorrowReturns
                        .Where(br => br.BorrowReturnDetails
                            .Any(bd => bd.BookId == b.BookId) && br.Status == "Borrowed") 
                        .Sum(br => br.BorrowReturnDetails
                            .Where(bd => bd.BookId == b.BookId)
                            .Sum(bd => bd.Quantity)) // Sum of borrowed quantities for each book
                })
                .OrderByDescending(b => b.BorrowCount)
                .Take(5)
                .ToList();
        }
    }
}