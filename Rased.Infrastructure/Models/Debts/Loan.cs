using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure.Models.Debts
{
    public class Loan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal InstallmentAmount { get; set; }
        public int TotalInstallments { get; set; }
        public int PaidInstallments { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Status { get; set; }   // C(Completed) - P(Progressing)
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //public char Frequency { get; set; }   // N(Normal) - D(Daily) - M(Monthly) - Y(Yearly)

        // Parents Ids
        public int WalletId { get; set; }

        // Navigation Properties
        public Wallet Wallet { get; set; } = new Wallet();
        public IEnumerable<LoanInstallment>? LoanInstallments { get; set; }
    }
}
