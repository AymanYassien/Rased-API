using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddTransactionDto
    {
        public string SenderId { get; set; }
        public int SenderWalletId { get; set; }
        public string? ReceiverId { get; set; }
        public int? ReceiverWalletId { get; set; }
        public int ReceiverTypeId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public int TransactionStatusId { get; set; }
        public string DisplayColor { get; set; }
        // New
        public int? RelatedBudgetId { get; set; }
        public int? SubCategoryId { get; set; }
    }
}
