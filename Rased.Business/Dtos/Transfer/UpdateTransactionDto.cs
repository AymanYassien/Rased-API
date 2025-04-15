using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Transfer
{
    public class UpdateTransactionDto : AddTransactionDto
    {
        public int TransactionId { get; set; }
    }

}
