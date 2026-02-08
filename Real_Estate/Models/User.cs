using Microsoft.AspNetCore.Identity;

namespace Real_Estate.Models
{
    public class User : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        //public string LastName { get; internal set; }
        //public string FirstName { get; internal set; }
    }

}
