using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class ResetPasswordDto
    {
        [Required(ErrorMessage = "البريد الإلكتروني مطلوب")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رقم المرور مطلوب")]
        [DataType(DataType.Password)]
        //[StringLength(100, ErrorMessage = "Password must be at least 6 characters long", MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "تأكيد رقم المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "رقم المرور لابد أن يكون مُطابق")]
        public string ConfirmedNewPassword { get; set; } = null!;
    }
}
