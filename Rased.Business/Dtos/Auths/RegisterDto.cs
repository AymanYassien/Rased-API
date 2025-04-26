using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "الاسم مطلوب")]
        [StringLength(255, ErrorMessage = "الإسم طويل جدا، حاول تقليل عدد الأحرف")]
        public string FullName { get; set; } = null!;

        [Required(ErrorMessage = "البريد الإلكتروني مطلوب!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "رقم المرور مطلوب")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "تأكيد رقم المرور مطلوب")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "لابد من تأكيد الرقم السري كما هو")]
        public string ConfirmedPassword { get; set; } = null!;
    }
}
