namespace Real_Estate.Models
{
    public class Feature : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<PropertyFeature> PropertyFeatures { get; set; }
    }

}
