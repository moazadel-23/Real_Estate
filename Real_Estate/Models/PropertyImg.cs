namespace Real_Estate.Models
{
    public class PropertyImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public bool IsMain { get; set; }

        public int PropertyId { get; set; }
        public Property Property { get; set; }
    }

}
