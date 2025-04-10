using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddSharedWalletIncomeTransactionDto
    {
        public int TransactionId { get; set; }
        public int IncomeId { get; set; }
        public int ApprovalId { get; set; }
        public int WalletId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetSharedWalletIncomeTransactionDto
    {
        public int SharedWalletIncomeTransactionId { get; set; }
        public int TransactionId { get; set; }
        public int IncomeId { get; set; }
        public int ApprovalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateSharedWalletIncomeTransactionDto
    {
        
        public int? ApprovalId { get; set; }
        public string IncomeSpecificData { get; set; }
    }

}
