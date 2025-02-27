namespace Rased.Infrastructure
{
    public class WalletStatistics
    {
        public int WalletId { get; set; }
        public decimal? TotalIncome { get; set; }
        public decimal? TotalExpenses { get; set; }
        public decimal? TotalSavings { get; set; }
        public decimal? TotalLoans { get; set; }
        
        public Wallet? Wallet { get; set; }
    }
}
