namespace Real_Estate.Models
{
    public enum PropertyType
    {
        Apartment,
        Villa,
        Office,
        Palace,
        Chalet
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

    }
}
