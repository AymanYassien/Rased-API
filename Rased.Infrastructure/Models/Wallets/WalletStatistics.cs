using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Rased.Infrastructure.Models.Wallets
{
    public class WalletStatistics
    {
        [Key]
        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal TotalLoans { get; set; }


        //public List<MonthlyReport> MonthlyReports { get; set; } = new List<MonthlyReport>();
        //public List<CategorySummary> CategorySummaries { get; set; } = new List<CategorySummary>();




    }
}
