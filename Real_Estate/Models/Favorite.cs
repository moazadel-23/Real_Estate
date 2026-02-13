using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Real_Estate.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public required string UserId { get; set; }
        public User? User { get; set; }

        [Required]
        public int PropertyId { get; set; }
        public Property Property { get; set; }=null!;
    }
}
