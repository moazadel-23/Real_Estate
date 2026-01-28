namespace Real_Estate.Models
{
    public class Property : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public double AreaSize { get; set; }

        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int Floor { get; set; }
        public int TotalFloors { get; set; }

        public bool IsActive { get; set; }

        public int PropertyTypeId { get; set; }
        public PropertyType? PropertyType { get; set; }

        public int ListingTypeId { get; set; }

        public int PropertyStatusId { get; set; }
        public PropertyStatus? PropertyStatus { get; set; }

        public int OwnerId { get; set; }
        public User? Owner { get; set; }

        public int? AgentId { get; set; }
        public Agent? Agent { get; set; }

        public int LocationId { get; set; }
        public Location? Location { get; set; }

        public ICollection<PropertyImage>? Images { get; set; }
        public ICollection<PropertyFeature>? PropertyFeatures { get; set; }

    }

}
