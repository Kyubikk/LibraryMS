using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;

namespace LoginSystemApp.Pages
{
    public class EditUserModel : PageModel
    {
        private readonly LibraryContext _context;

        public EditUserModel(LibraryContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; }

        public IActionResult OnGet(int id)
        {
            User = _context.Users.Find(id);
            if (User == null)
            {
                Console.WriteLine($"User with ID {id} not found.");
                return NotFound();
            }
            Console.WriteLine($"Loaded User: Id={User.Id}, Username={User.Username}, Role={User.Role}");
            return Page();
        }

        public IActionResult OnPost()
        {
            // Loại bỏ lỗi của trường Password
            ModelState.Remove("User.Password");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid.");
                foreach (var modelStateEntry in ModelState)
                {
                    Console.WriteLine($"Key: {modelStateEntry.Key}");
                    foreach (var error in modelStateEntry.Value.Errors)
                    {
                        Console.WriteLine($"Error: {error.ErrorMessage}");
                    }
                }
                return Page();
            }

            var userInDb = _context.Users.Find(User.Id);
            if (userInDb == null)
            {
                Console.WriteLine($"User with ID {User.Id} not found in database.");
                return NotFound();
            }

            userInDb.Username = User.Username;
            userInDb.Role = User.Role;

            _context.SaveChanges();

            Console.WriteLine($"User updated: Id={userInDb.Id}, Username={userInDb.Username}, Role={userInDb.Role}");

            return RedirectToPage("/Admin");
        }

    }
}
