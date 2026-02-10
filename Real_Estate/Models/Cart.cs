using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Real_Estate.Models
{
    public class Cart
    {
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}