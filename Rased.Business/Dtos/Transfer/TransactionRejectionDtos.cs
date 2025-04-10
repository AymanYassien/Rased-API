using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddTransactionRejectionDto
    {
        public int TransactionId { get; set; }
        public string RejectedBy { get; set; }
        public string? RejectionReason { get; set; }
    }

    public class UpdateTransactionRejectionDto
    {
        public int RejectionId { get; set; }
        public string? NewRejectionReason { get; set; }
    }

    public class ReadTransactionRejectionDto
    {
        public int RejectionId { get; set; }
        public int TransactionId { get; set; }
        public string RejectedBy { get; set; }
        public string? RejectionReason { get; set; }
        public DateTime RejectedAt { get; set; }
      
    }



}
