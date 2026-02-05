namespace Real_Estate.Models
{
    public class PropertySubImage : BaseEntity
    {
        public string PropertyImgs { get; set; } = string.Empty;
        public int PropertyId { get; set; }
        public Property? Property { get; set; }
    }

}
