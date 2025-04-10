using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class AddPersonalIncomeTransactionRecordDto
    {
        public int TransactionId { get; set; }
        public int IncomeId { get; set; }
        public int ApprovalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class GetPersonalIncomeTransactionRecordDto
    {
        public int PersonalIncomeTrasactionRecordId { get; set; }
        public int TransactionId { get; set; }
        public int IncomeId { get; set; }
        public int ApprovalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? IncomeSpecificData { get; set; }
    }

    public class UpdatePersonalIncomeTransactionRecordDto
    {
     
        public string? IncomeSpecificData { get; set; }
    }
}
