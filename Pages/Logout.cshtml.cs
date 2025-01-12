using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;

public class LogoutModel : PageModel
{
    public async Task<IActionResult> OnPostLogoutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        // Sau khi đăng xuất, chuyển hướng người dùng về trang chính (Home)
        return RedirectToPage("/Index"); 
    }
}
