namespace Real_Estate.Models
{
    [PrimaryKey(nameof(Otp) , nameof(UserId))]
    public class UserOtp
    {
        public int Otp { get; set; }
         public string UserId { get; set; } = null!;
        public User? User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpireAt { get; set; }
    }
}
