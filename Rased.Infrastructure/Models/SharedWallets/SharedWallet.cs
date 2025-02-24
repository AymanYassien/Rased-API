using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.Budgets;
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Expenses;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Incomes;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.Transfers;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
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
        public StaticWalletStatusData Status { get; set; }


        // Navigation properties
        public virtual RasedUser Creator { get; set; }
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