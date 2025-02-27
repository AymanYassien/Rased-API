
namespace Rased.Infrastructure
{
    public class ExpenseTransactionRecord
    {

        public int ExpenseTrasactionRecordId { get; set; }
        public int TransactionId { get; set; }
        public int ExpenseId { get; set; }
        public bool isDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string ExpenseSpecificData { get; set; }

        // Navigation Property
        public virtual Transaction Transaction { get; set; }
        public virtual Expense Expense { get; set; }

    }
}