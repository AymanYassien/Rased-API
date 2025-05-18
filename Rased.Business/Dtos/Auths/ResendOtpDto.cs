using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class ResendOtpDto
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
