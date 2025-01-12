using LoginSystemApp.Data;
using LoginSystemApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;

public class LoginModel : PageModel
{
    private readonly LibraryContext _context;

    public LoginModel(LibraryContext context)
    {
        _context = context;
    }

    [BindProperty]
    public string Username { get; set; }
    [BindProperty]
    public string Password { get; set; }
    public string ErrorMessage { get; set; }

    public async Task<IActionResult> OnPostAsync()
    {
        // Mã hóa mật khẩu
        var hashedPassword = BitConverter.ToString(
            System.Security.Cryptography.SHA256.HashData(
                System.Text.Encoding.UTF8.GetBytes(Password)
            )).Replace("-", "");

        // Kiểm tra người dùng trong cơ sở dữ liệu
        var user = _context.Users.FirstOrDefault(u => u.Username == Username && u.Password == hashedPassword);

        if (user != null)
        {
            // Tạo các claim cho người dùng
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role) // Thêm role để phân quyền
            };

            // Đăng nhập người dùng và tạo cookie
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // Lưu trữ thông tin người dùng trong cookie
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            // Chuyển hướng về trang sau khi đăng nhập thành công
            return RedirectToPage("/Books/Index", new { username = user.Username });
        }

        // Nếu đăng nhập thất bại
        ErrorMessage = "Invalid username or password.";
        return Page();
    }
}
