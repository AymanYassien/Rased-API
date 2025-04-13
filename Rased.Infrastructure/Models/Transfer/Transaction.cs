
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{

    public class Transaction
    {
        public int TransactionId { get; set; }
        public string SenderId { get; set; }
        public int SenderWalletId { get; set; }
        public string? ReceiverId { get; set; }
        public int? ReceiverWalletId { get; set; }
        public int ReceiverTypeId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int TransactionStatusId { get; set; }
        public string DisplayColor { get; set; }
        public bool IsReadOnly { get; set; } = false;


        // Navigation properties
        public virtual RasedUser Sender { get; set; }
        public virtual StaticTransactionStatusData StaticTransactionStatusData { get; set; }
        public virtual StaticReceiverTypeData StaticReceiverTypeData { get; set; }
        public virtual SharedWallet SenderWallet { get; set; }
        public virtual RasedUser Receiver { get; set; }
        public virtual SharedWallet ReceiverWallet { get; set; }

    }

}