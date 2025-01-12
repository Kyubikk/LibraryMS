using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoginSystemApp.Models
{
    [Table("BorrowReturn")] 
    public class BorrowReturnTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required(ErrorMessage = "Borrow Date is required.")]
        public DateTime BorrowDate { get; set; }

        [Required(ErrorMessage = "Due Date is required.")]
        public DateTime DueDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Borrower Name is required.")]
        [StringLength(255, ErrorMessage = "Borrower Name cannot exceed 255 characters.")]
        public string BorrowerName { get; set; }

        [Required(ErrorMessage = "Borrower Contact is required.")]
        [StringLength(100, ErrorMessage = "Borrower Contact cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@vnu\.edu\.vn$", ErrorMessage = "Borrower Contact must be a valid email ending with @vnu.edu.vn.")]
        public string BorrowerContact { get; set; }

        public string Status { get; set; } = "Borrowed"; 

        [Range(0, double.MaxValue, ErrorMessage = "Fine cannot be negative.")]
        public decimal? Fine { get; set; } 

        // Navigation property
        public ICollection<BorrowReturnDetail> BorrowReturnDetails { get; set; }
    }
}
