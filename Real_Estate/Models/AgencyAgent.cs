namespace Real_Estate.Models
{
    public class AgencyAgent
    {
        public int AgencyId { get; set; }
        public Agency? Agency { get; set; }

        public int AgentId { get; set; }
        public Agent? Agent { get; set; }
    }

}
