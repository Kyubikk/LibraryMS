using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Linq;

namespace LoginSystemApp.Pages.Books
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
        public Book Book { get; set; }

        [BindProperty]
        public IFormFile? CoverImage { get; set; }

        public SelectList Authors { get; set; }
        public SelectList Publishers { get; set; }
        public SelectList Categories { get; set; }

        public IActionResult OnGet(int id)
        {
            Book = _context.Books.FirstOrDefault(b => b.BookId == id);

            if (Book == null)
            {
                return NotFound();
            }

            Authors = new SelectList(_context.Authors, "AuthorId", "Name");
            Publishers = new SelectList(_context.Publishers, "PublisherId", "Name");
            Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (!ModelState.IsValid)
            {
                Authors = new SelectList(_context.Authors, "AuthorId", "Name");
                Publishers = new SelectList(_context.Publishers, "PublisherId", "Name");
                Categories = new SelectList(_context.Categories, "CategoryId", "CategoryName");
                return Page();
            }

            var bookInDb = _context.Books.FirstOrDefault(b => b.BookId == id);
            if (bookInDb == null)
            {
                return NotFound();
            }

            bookInDb.Title = Book.Title;
            bookInDb.AuthorId = Book.AuthorId;
            bookInDb.PublisherId = Book.PublisherId;
            bookInDb.CategoryId = Book.CategoryId;
            bookInDb.YearPublished = Book.YearPublished;
            bookInDb.ISBN = Book.ISBN;
            bookInDb.Quantity = Book.Quantity;
            bookInDb.Available = Book.Available;

            // Nếu có ảnh bìa, xử lý upload
            if (CoverImage != null)
            {
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(CoverImage.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("CoverImage", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                    return Page();
                }

                var uploadsFolder = Path.Combine("wwwroot/uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + CoverImage.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    CoverImage.CopyTo(stream);
                }

                bookInDb.CoverImagePath = "/uploads/" + uniqueFileName;
            }

            // Đặt trạng thái là Modified để cập nhật
            _context.Attach(bookInDb).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            // Lưu thay đổi vào database
            _context.SaveChanges();

            return RedirectToPage("./Index");
        }
    }
}
