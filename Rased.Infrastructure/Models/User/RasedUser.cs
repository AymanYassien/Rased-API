using Microsoft.AspNetCore.Identity;

namespace Rased.Infrastructure.Models.User
{
    public class RasedUser: IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public byte[]? ProfilePic { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
