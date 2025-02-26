using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Transfers
{

    public class Transaction
    {
        public int TransactionId { get; set; }
        public int SenderId { get; set; }
        public int SenderWalletId { get; set; }
        public int? ReceiverId { get; set; }
        public int? ReceiverWalletId { get; set; }
        public string? ReceiverType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? Status { get; set; }
        public string DisplayColor { get; set; }
        public bool IsReadOnly { get; set; }


        // Navigation properties
        public virtual RasedUser Sender { get; set; }
        public virtual SharedWallet SenderWallet { get; set; }
        public virtual RasedUser Receiver { get; set; }
        public virtual SharedWallet ReceiverWallet { get; set; }

    }

}