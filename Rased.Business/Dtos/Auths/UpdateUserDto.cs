
using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Auths
{
    public class UpdateUserDto
    {
        [Required]
        public string FullName { get; set; } = null!;
        [Required]
        public string UserName { get; set; } = null!;
        public string? Address { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ProfilePic { get; set; }
    }
}
