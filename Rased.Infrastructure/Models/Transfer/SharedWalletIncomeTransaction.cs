namespace Rased.Infrastructure
{
    public class SharedWalletIncomeTransaction
    {

        public int SharedWalletIncomeTransactionId { get; set; }
        public int TransactionId { get; set; }
        public int IncomeId { get; set; }
        public int ApprovalId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public virtual Transaction Transaction { get; set; }
        public virtual Income Income { get; set; }
        public virtual TransactionApproval TransactionApproval { get; set; }
    }

}