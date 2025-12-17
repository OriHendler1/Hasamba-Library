using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hasamba_Library.Model
{
    public class Reader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int readerId {  get; set; }

        [Required]
        public string name { get; set; }

        public int NumberOfAvailableLoans { get; set; } = 5;
    }
}
