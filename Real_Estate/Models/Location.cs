namespace Real_Estate.Models
{
    public class Location : BaseEntity
    {
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Area { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public ICollection<Property> Properties { get; set; } = new List<Property>();
    }

}
