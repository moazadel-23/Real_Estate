using System.ComponentModel.DataAnnotations;

namespace Real_Estate.Models.ViewModel
{
    public class RegisterVM
    {
        public string Name { get; set; } = null!;
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [DataType(DataType.Password) , Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = null!;
    }
}
