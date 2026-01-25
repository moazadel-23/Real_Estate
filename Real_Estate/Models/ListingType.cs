namespace Real_Estate.Models
{
    public class ListingType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }
}
