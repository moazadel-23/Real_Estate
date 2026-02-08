namespace Real_Estate.Models
{
    public class PropertyLocVM
    {
        public Property Property { get; set; } = new Property();
        public Location  Location { get; set; } = new Location();
        public PropertyType type { get; set; } = new PropertyType();
    }
}
