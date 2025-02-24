using Rased.Infrastructure.Models.Budgets;
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Expenses;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Incomes;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Wallets
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public string WalletStatus { get; set; }
        public string ColorType { get; set; }
        public string CurrencyType { get; set; }

        public virtual RasedUser User { get; set; } = new RasedUser();
        public virtual ICollection<Income> Incomes { get; set; } = new HashSet<Income>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual ICollection<ExpenseTemplate> ExpenseTemplates { get; set; } = new List<ExpenseTemplate>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
    }
}
