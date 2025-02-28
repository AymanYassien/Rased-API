namespace Rased.Infrastructure.Models.Debts
{
    public class LoanInstallment
    {
        public int Id { get; set; }
        public decimal AmountToPay { get; set; }
        public DateTime DateToPay { get; set; }
        public string? Status { get; set; }   // C(Completed) - P(Pending) - Outdated
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int LoanId { get; set; }

        // Navigation Properties
        public Loan Loan { get; set; } = null!;
    }
}
