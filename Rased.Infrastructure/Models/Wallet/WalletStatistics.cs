using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Wallets
{
    public class WalletStatistics
    {
        public int WalletId { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal TotalLoans { get; set; }
        
        public Wallet Wallet { get; set; }
    }
}
