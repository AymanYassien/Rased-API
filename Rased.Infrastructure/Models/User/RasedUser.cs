using Microsoft.AspNetCore.Identity;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.Preferences;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.Subscriptions;

namespace Rased.Infrastructure.Models.User
{
    public class RasedUser: IdentityUser
    {
        public string FullName { get; set; } = null!;
        public DateOnly? DateOfBirth { get; set; }
        public byte[]? ProfilePic { get; set; }
        public string? Country { get; set; }
        public string? Address { get; set; }
        public string UserBadge { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string AccountStatus { get; set; } = null!; // Active - InActive - Suspended
        public string? OTP { get; set; }
        public DateTime? OtpExpiryTime { get; set; }

        public bool IsBanned { get; set; }
        public string? BannedDuration { get; set; } // 15m, 1h, 1d, forever 
        public DateTime? BanBrokeAt { get; set; } // When the user broke the ban -- Null = Forever
        public string? BannedReason { get; set; } // Too Many Failed Attempts, etc
        public int FailedAttempts { get; set; } // Number of failed attempts to login

        //public string? RefreshToken { get; set; }
        //public DateTime? RefreshTokenExpiryTime { get; set; }

        // Navigation Properties
        public virtual UserPreference? Preference { get; set; }
        public virtual ICollection<Notification>? Notifications { get; set; }

        public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        // Has Many Individual Wallets
        public virtual ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
        // Has Many Shared Wallet with different access level
        public virtual ICollection<SharedWallet> SharedWallets { get; set; } = new List<SharedWallet>();
        // Has Many Membership
        public virtual ICollection<SharedWalletMembers> SWMembers { get; set; } = new List<SharedWalletMembers>();
        // Shared Wallets Invitaions
        public virtual ICollection<SWInvitation> Senders { get; set; } = new List<SWInvitation>();
        public virtual ICollection<SWInvitation> Receivers { get; set; } = new List<SWInvitation>();
        // Friendships requests 
        public virtual ICollection<Friendship> FriendSenders { get; set; } = new List<Friendship>();
        public virtual ICollection<Friendship> FriendReceivers { get; set; } = new List<Friendship>();
    }
}
