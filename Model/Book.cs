using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hasamba_Library.Model
{
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookID { get; set; }

        [Required]
        public string BookName { get; set; }

        public string ?Description { get; set; }

        [Required]
        public string Author { get; set; }
        
        [Required]
        [Range(0, int.MaxValue)]
        public int NumberOfAvailableCopies { get; set; }
    }
}
