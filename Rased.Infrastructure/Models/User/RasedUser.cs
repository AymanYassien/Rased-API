using Microsoft.AspNetCore.Identity;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.Preferences;
using Rased.Infrastructure.Models.Subscriptions;

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

        public string? OTP { get; set; }
        public DateTime? OtpExpiryTime { get; set; }
        public bool IsBanned { get; set; } = false;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }




        // Navigation Properties
        public virtual UserPreference? Preference { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
        public virtual ICollection<SharedWallet> SharedWallets { get; set; } = new List<SharedWallet>();
        public virtual ICollection<Notification>? Notifications { get; set; }
    }
}
