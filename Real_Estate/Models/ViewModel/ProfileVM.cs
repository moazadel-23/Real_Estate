using System.ComponentModel.DataAnnotations;

namespace Real_Estate.Models.ViewModel
{
    public class ProfileVM
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
    }
}
