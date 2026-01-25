namespace Real_Estate.Models
{
    public class Favorite
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }

}
