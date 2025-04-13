using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
{
    public class SWInvitation
    {
        public int Id { get; set; }
        public int SharedWalletId { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string Status { get; set; } = null!; // "Pending", "Accepted", "Declined"
        public DateTime InvitedAt { get; set; }

        // Navigation properties
        public virtual SharedWallet SharedWallet { get; set; } = null!;
        public virtual RasedUser Sender { get; set; } = null!;
        public virtual RasedUser Receiver { get; set; } = null!;
    }
}
