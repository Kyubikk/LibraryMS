using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace LoginSystemApp.Pages.Authors
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
        public Author Author { get; set; }

        public IActionResult OnGet(int id)
        {
            // Tìm tác giả dựa trên id
            Author = _context.Authors.FirstOrDefault(a => a.AuthorId == id);

            if (Author == null)
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

            // Tìm tác giả trong cơ sở dữ liệu
            var authorInDb = _context.Authors.FirstOrDefault(a => a.AuthorId == id);

            if (authorInDb == null)
            {
                return NotFound();
            }

            // Cập nhật thông tin từ dữ liệu người dùng
            authorInDb.Name = Author.Name;
            authorInDb.Bio = Author.Bio;
            authorInDb.DateOfBirth = Author.DateOfBirth;

            // Đánh dấu trạng thái Modified
            _context.Attach(authorInDb).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            // Lưu thay đổi vào cơ sở dữ liệu
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Author updated successfully!";
            return RedirectToPage("./Index");
        }
    }
}
