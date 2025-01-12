using Microsoft.AspNetCore.Mvc.RazorPages;
using LoginSystemApp.Data;
using LoginSystemApp.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace LoginSystemApp.Pages
{
    public class AdminModel : PageModel
    {
        private readonly LibraryContext _context;

        public AdminModel(LibraryContext context)
        {
            _context = context;
        }

        public List<User> Users { get; set; }

        public void OnGet()
        {
            Users = _context.Users.ToList();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Deleted User: Id={user.Id}, Username={user.Username}");
            }
            else
            {
                Console.WriteLine($"User with ID {id} not found.");
            }

            return RedirectToPage();
        }
    }
}
