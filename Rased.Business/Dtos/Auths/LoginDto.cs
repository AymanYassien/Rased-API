using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class LoginDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "الرقم السري مطلوب")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
