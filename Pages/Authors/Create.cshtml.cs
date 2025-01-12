using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using Microsoft.AspNetCore.Authorization;
using LoginSystemApp.Models;

namespace LoginSystemApp.Pages.Authors
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
        public Author Author { get; set; }

        public void OnGet()
        {
            // Phương thức để tải trang Create
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            try
            {
                Console.WriteLine($"Adding Author: {Author.Name}");
                _context.Authors.Add(Author);
                _context.SaveChanges();
                Console.WriteLine("Author added successfully!");
                TempData["SuccessMessage"] = "Author added successfully!";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                ModelState.AddModelError(string.Empty, "An error occurred while saving the author.");
                return Page();
            }
        }
    }
}
