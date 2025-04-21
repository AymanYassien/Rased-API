using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
{
    public class SharedWalletMembers
    {
        public int MembershipId { get; set; }
        public string Role { get; set; } = null!;
        public DateTime JoinedAt { get; set; }

        // Parents
        public int SharedWalletId { get; set; }
        public string UserId { get; set; } = null!;

        // Navigation properties
        public virtual SharedWallet SharedWallet { get; set; } = null!;
        public virtual RasedUser Member { get; set; } = null!;
    }
}