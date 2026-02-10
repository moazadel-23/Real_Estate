namespace Real_Estate.Models.ViewModel
{
    public class PropertyFilterVM
    {
    
        public PropertyType? PropertyType { get; set; }
        
        public decimal? MaxPrice { get; set; }

        public string?SearchLocation { get; set; }

        public List<Property>? Properties { get; set; }
    }
}
