
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{
    public class SharedWallet
    {
        public int SharedWalletId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public string Currency { get; set; }
        public decimal TotalBalance { get; set; }
        public int CreatorId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastModified { get; set; }
        public int WalletStatusId { get; set; }
        public virtual RasedUser Creator { get; set; }
        public virtual StaticWalletStatusData StaticWalletStatusData { get; set; }
        public virtual ICollection<SharedWalletMembers> Members { get; set; }
        public virtual ICollection<Income> Incomes { get; set; }
        public virtual ICollection<Expense> Expenses { get; set; }
        public virtual ICollection<ExpenseTemplate> ExpensesTemplates { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Goal> Goals { get; set; }
        public virtual ICollection<Saving> Savings { get; set; }
        public virtual ICollection<Transaction> SentTransactions { get; set; }
        public virtual ICollection<Transaction> ReceivedTransactions { get; set; }

    }
}