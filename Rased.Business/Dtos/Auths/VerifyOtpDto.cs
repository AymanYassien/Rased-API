using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class VerifyOtpDto
    {
        public bool RememberMe { get; set; }
        [Required]
        public string Email { get; set; } = null!;
        [Required]
        public string OTP { get; set; } = null!;
    }
}
