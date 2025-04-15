using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddTransactionApprovalDto
    {
        public int TransactionId { get; set; }
        public string ApproverId { get; set; }
        public DateTime ApprovedAt { get; set; } = DateTime.UtcNow;
    }

    public class ReadTransactionApprovalDto
    {
        public int ApprovalId { get; set; }
        public int TransactionId { get; set; }
        public string ApproverId { get; set; }
        public DateTime ApprovedAt { get; set; }
    }

    public class TransactionApprovalDto
    {
        public int TransactionId { get; set; }
        public string ApproverId { get; set; }
        public int? ReceiverWalletId { get; set; } // only for personal users
    }



}
