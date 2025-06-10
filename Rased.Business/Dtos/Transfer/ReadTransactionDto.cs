using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class ReadTransactionDto
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
        public int TransactionStatusId { get; set; }
        public string DisplayColor { get; set; }

       
    }
    public class ReadTransactionForSenderDto
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
        public string DisplayColor { get; set; }
        public int? RelatedBudgetId { get; set; }
        public int? SubCategoryId { get; set; }
    }

    public class ReadTransactionForReceiverDto
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
        public int TransactionStatusId { get; set; }
        public string DisplayColor { get; set; }
    }
}
