namespace Real_Estate.Models
{
    public class Feature : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        public ICollection<PropertyFeature> PropertyFeatures { get; set; } = new List<PropertyFeature>();
    }

}
