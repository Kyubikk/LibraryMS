using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace LoginSystemApp.Pages.BorrowReturn
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly LibraryContext _context;
        public string StatusFilter { get; set; } = string.Empty;

        public IndexModel(LibraryContext context)
        {
            _context = context;
        }

        public PaginatedList<BorrowReturnTransaction> BorrowReturns { get; set; } = default!;
        public string SearchString { get; set; } = string.Empty;

    public void OnGet(string? searchString, string? statusFilter, int? pageIndex)
    {
        SearchString = searchString ?? string.Empty;
        StatusFilter = statusFilter ?? string.Empty;

        var query = _context.BorrowReturns
            .Include(br => br.BorrowReturnDetails)
            .ThenInclude(detail => detail.Book)
            .AsQueryable();

        // Lọc theo chuỗi tìm kiếm
        if (!string.IsNullOrEmpty(SearchString))
        {
            query = query.Where(br =>
                br.TransactionId.ToString().Contains(SearchString) ||
                br.BorrowerName.Contains(SearchString) ||
                br.BorrowerContact.Contains(SearchString));
        }

        // Lọc theo trạng thái
        if (!string.IsNullOrEmpty(StatusFilter))
        {
            query = query.Where(br => br.Status == StatusFilter);
        }

        query = query.OrderBy(br => br.TransactionId);

        int pageSize = 10;
        BorrowReturns = PaginatedList<BorrowReturnTransaction>.Create(query, pageIndex ?? 1, pageSize);
    }

        public JsonResult OnPostMarkAsReturned([FromBody] MarkAsReturnedRequest request)
        {
            if (request.TransactionId == 0)
            {
                return new JsonResult(new { success = false, message = "Invalid Transaction ID." });
            }

            var transaction = _context.BorrowReturns.FirstOrDefault(br => br.TransactionId == request.TransactionId);
            if (transaction == null)
            {
                return new JsonResult(new { success = false, message = $"Transaction with ID {request.TransactionId} not found." });
            }

            if (transaction.Status == "Returned")
            {
                return new JsonResult(new { success = false, message = "Transaction already returned." });
            }

            transaction.Status = "Returned";
            transaction.ReturnDate = DateTime.Now;
            _context.SaveChanges();

            return new JsonResult(new { success = true });
        }

        public JsonResult OnGetDetails(int transactionId)
        {
            var transaction = _context.BorrowReturns
                .Include(br => br.BorrowReturnDetails)
                .ThenInclude(detail => detail.Book)
                .FirstOrDefault(br => br.TransactionId == transactionId);

            if (transaction == null)
            {
                return new JsonResult(new { success = false, message = "Transaction not found." });
            }

            return new JsonResult(new
            {
                success = true,
                transactionId = transaction.TransactionId,
                borrowerName = transaction.BorrowerName,
                borrowerContact = transaction.BorrowerContact,
                borrowDate = transaction.BorrowDate.ToString("yyyy-MM-dd"),
                dueDate = transaction.DueDate.ToString("yyyy-MM-dd"),
                status = transaction.Status,
                fine = transaction.Fine,
                books = transaction.BorrowReturnDetails.Select(detail => new
                {
                    title = detail.Book?.Title ?? "Unknown Title",
                    quantity = detail.Quantity
                })
            });
        }
    }

    public class MarkAsReturnedRequest
    {
        public int TransactionId { get; set; }
    }
}
