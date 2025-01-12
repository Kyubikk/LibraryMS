using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace LoginSystemApp.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }

        [BindNever] 
        public string Password { get; set; }
        
        public string Role { get; set; }
    }
}
