using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.IO;

namespace LoginSystemApp.Pages.Books
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly LibraryContext _context;

        public CreateModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty] 
        public Book Book { get; set; }

        [BindProperty] // NET Core automatically attaches uploaded files to the CoverImage property
        public IFormFile? CoverImage { get; set; }

        public SelectList Categories { get; set; }
        public SelectList Authors { get; set; }
        public SelectList Publishers { get; set; }

        public void OnGet()
        {
            Categories = new SelectList(
                _context.Categories.Where(c => c.CategoryName != null).ToList(),
                "CategoryId", "CategoryName"
            );
            Authors = new SelectList(
                _context.Authors.Where(a => a.Name != null).ToList(),
                "AuthorId", "Name"
            );
            Publishers = new SelectList(
                _context.Publishers.Where(p => p.Name != null).ToList(),
                "PublisherId", "Name"
            );
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid. Details:");
                foreach (var key in ModelState.Keys)
                {
                    var errors = ModelState[key].Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Key: {key}, Error: {error.ErrorMessage}");
                    }
                }
                ReloadDropdowns();
                return Page();
            }

            try
            {
                if (CoverImage != null)
                {
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                    var extension = Path.GetExtension(CoverImage.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("CoverImage", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                        ReloadDropdowns();
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

                    Book.CoverImagePath = "/uploads/" + uniqueFileName;
                }

                Book.Available = Book.Quantity;

                _context.Books.Add(Book);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Book added successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the book.");
                ReloadDropdowns();
                return Page();
            }
        }

        private void ReloadDropdowns()
        {
            Categories = new SelectList(
                _context.Categories.Where(c => c.CategoryName != null).ToList(),
                "CategoryId", "CategoryName"
            );
            Authors = new SelectList(
                _context.Authors.Where(a => a.Name != null).ToList(),
                "AuthorId", "Name"
            );
            Publishers = new SelectList(
                _context.Publishers.Where(p => p.Name != null).ToList(),
                "PublisherId", "Name"
            );
        }
    }
}
