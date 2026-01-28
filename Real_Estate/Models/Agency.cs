namespace Real_Estate.Models
{
    public class Agency : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public ICollection<AgencyAgent> AgencyAgents { get; set; } = new List<AgencyAgent>();
    }

}
