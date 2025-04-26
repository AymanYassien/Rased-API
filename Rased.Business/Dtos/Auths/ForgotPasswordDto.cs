using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = null!;
    }
}
