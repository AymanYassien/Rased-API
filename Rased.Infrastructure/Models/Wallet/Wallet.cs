using Rased.Business;
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;


namespace Rased.Infrastructure
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
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public int WalletStatusId { get; set; }
        public int ColorTypeId { get; set; }
        public int CurrencyTypeID { get; set; }

        public virtual StaticWalletStatusData StaticWalletStatusData { get; set; }
        public virtual StaticColorTypeData StaticColorTypeData { get; set; }
        public virtual StaticCurrencyTypeData StaticCurrencyTypeData { get; set; } 
        public virtual ICollection<Income> Incomes { get; set; } = new HashSet<Income>();
        public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
        public virtual ICollection<ExpenseTemplate> ExpensesTemplate { get; set; } = new HashSet<ExpenseTemplate>();
        public virtual ICollection<Budget> Budgets { get; set; } = new HashSet<Budget>();
        public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
        public virtual ICollection<Goal> Goals { get; set; } = new HashSet<Goal>();
        public virtual ICollection<Saving> Savings { get; set; } = new HashSet<Saving>();
        public virtual ICollection<WalletNotification> Notifications { get; set; }
        

    }
}
