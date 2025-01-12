using Microsoft.EntityFrameworkCore;
using LoginSystemApp.Models;
 
namespace LoginSystemApp.Data
{
    public class LibraryContext : DbContext
    {
        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowReturnTransaction> BorrowReturns { get; set; }
        public DbSet<BorrowReturnDetail> BorrowReturnDetails { get; set; }
        public DbSet<Librarian> Librarians { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
