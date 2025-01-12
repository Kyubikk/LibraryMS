using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LoginSystemApp.Pages
{
    public class WelcomeModel : PageModel
    {
        [BindProperty(SupportsGet = true)] // Cho phép bind giá trị từ query string hoặc route
        public string Username { get; set; }

        public void OnGet()
        {
            // Không cần xử lý thêm, giá trị Username được tự động bind từ query string hoặc route
        }
    }
}
