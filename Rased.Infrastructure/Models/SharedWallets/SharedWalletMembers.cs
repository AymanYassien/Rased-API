using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
{

    public class SharedWalletMembers
    {
        public int MembershipId { get; set; }
        public int SharedWalletId { get; set; }
        public string UserId { get; set; } = null!;
        public int AccessLevelId { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation properties
        public virtual SharedWallet SharedWallet { get; set; } = null!;
        public virtual StaticSharedWalletAccessLevelData StaticSharedWalletAccessLevelData { get; set; } = null!;
        public virtual RasedUser Member { get; set; } = null!;
    }
}