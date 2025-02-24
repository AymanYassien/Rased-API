using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
{

    public class SharedWalletMembers
    {
        public int MembershipId { get; set; }
        public int SharedWalletId { get; set; }
        public int UserId { get; set; }
        public string? AccessLevel { get; set; }
        public DateTime JoinedAt { get; set; }

        // Navigation properties
        public virtual SharedWallet Wallet { get; set; }
        public virtual RasedUser UserProfile { get; set; }
    }
}