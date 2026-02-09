namespace Real_Estate.Models
{
    public enum PropertyType
    {
        None = 0,
        Apartment = 1,
        Villa = 2,
        Office = 3,
        Palace = 4,
        Chalet = 5
    }
    public class Property : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MainImg { get; set; } = string.Empty;
        public PropertyType Type { get; set; }
        public decimal Price { get; set; }
        public double AreaSize { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Floor { get; set; }
        public bool IsActive { get; set; }
        public int LocationId { get; set; }
        public Location? Location { get; set; }
        public ICollection<Models.PropertySubImage> PropertySubImgs { get; set; } = new List<Models.PropertySubImage>();

    }
}
