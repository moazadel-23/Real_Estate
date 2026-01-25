namespace Real_Estate.Models
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordHash { get; set; }

        public bool IsActive { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public Agent Agent { get; set; }

        public ICollection<Property> Properties { get; set; }
    }

}
