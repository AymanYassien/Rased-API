namespace Rased.Infrastructure.Models.Debt
{
    public class LoanInstallment
    {
        public int Id { get; set; }
        public decimal AmountToPay { get; set; }
        public DateTime DateToPay { get; set; }
        public bool IsOutDated { get; set; }
        public char Status { get; set; }   // C(Completed) - P(Pending)

        // Parents Ids
        public int LoanId { get; set; }

        // Navigation Properties
        public Loan Loan { get; set; } = null!;
    }
}
