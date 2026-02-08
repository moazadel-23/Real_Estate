using System.ComponentModel.DataAnnotations;

namespace Real_Estate.Models.ViewModel
{
    public class ResetPasswordVM
    {
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        public string UserId { get; set; } = string.Empty;
    }
}
