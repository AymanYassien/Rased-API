
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{

    public class SharedWalletMembers
    {
        public int MembershipId { get; set; }
        public int SharedWalletId { get; set; }
        public int UserId { get; set; }
        public int AccessLevelId { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation properties
        public virtual SharedWallet Wallet { get; set; }
        public virtual StaticSharedWalletAccessLevelData StaticSharedWalletAccessLevelData { get; set; }
        public virtual RasedUser UserProfile { get; set; }
    }
}