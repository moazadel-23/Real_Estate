namespace Real_Estate.Models
{
    public class PropertyLocVM
    {
        public Property Property { get; set; } = new Property();
        public Location  Location { get; set; } = new Location();
        public PropertySubImage PropertySubImage { get; set; } = new PropertySubImage();
        public PropertyType type { get; set; } = new PropertyType();
    }
}
