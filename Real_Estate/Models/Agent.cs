namespace Real_Estate.Models
{
    public class Agent : BaseEntity
    {
        public int UserId { get; set; }
        public User? User { get; set; }

        public string LicenseNumber { get; set; } = string.Empty;
        public int ExperienceYears { get; set; }
        public double Rating { get; set; }

        public ICollection<Property>? Properties { get; set; }
        public ICollection<AgencyAgent>? AgencyAgents { get; set; }
    }

}
