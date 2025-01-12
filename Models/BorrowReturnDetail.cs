using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystemApp.Models
{
    [Table("BorrowReturnDetails")]
    public class BorrowReturnDetail
    {
        [Key]
        [Column("DetailId")] 
        public int DetailId { get; set; }

        [ForeignKey("BorrowReturnTransaction")]
        public int TransactionId { get; set; }

        [ForeignKey("Book")]
        public int BookId { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        // Navigation properties
        public BorrowReturnTransaction BorrowReturnTransaction { get; set; }
        public Book Book { get; set; }
    }
}
