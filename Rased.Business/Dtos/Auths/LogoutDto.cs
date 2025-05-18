using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class LogoutDto
    {
        [Required]
        public string Email { get; set; } = null!;
    }
}
