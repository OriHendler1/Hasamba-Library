using System.ComponentModel.DataAnnotations;

namespace Hasamba_Library.Model
{
    public class Loan
    {
        [Key]
        [Required]
        public int LoanId { get; set; }
        
        [Required]
        public int ReaderId { get; set; }
        
        [Required]
        public int BookId { get; set; }

        [Required]
        public LoanStatusState LoanStatus { get; set; }
        
        public enum LoanStatusState : byte
        {
            Borrowed = 1,
            Returned = 2
        }
    }
}
