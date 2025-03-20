using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.User;


namespace Rased.Infrastructure
{
    public class Wallet
    {
        
        public int WalletId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public int WalletStatusId { get; set; }
        public int ColorTypeId { get; set; }
        public int CurrencyId { get; set; }
        public string CreatorId { get; set; } = null!;

        // Navigation Properties
        public virtual Currency Currency { get; set; } = new Currency();
        public virtual RasedUser Creator { get; set; } = new RasedUser();
        public virtual WalletStatistics? WalletStatistics { get; set; }
        public virtual StaticWalletStatusData StaticWalletStatusData { get; set; } = new StaticWalletStatusData();
        public virtual StaticColorTypeData StaticColorTypeData { get; set; } = new StaticColorTypeData();
        public virtual ICollection<Income>? Incomes { get; set; }
        public virtual ICollection<IncomeTemplate>? IncomeTemplates { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
        public virtual ICollection<ExpenseTemplate>? ExpensesTemplate { get; set; }
        public virtual ICollection<Budget>? Budgets { get; set; }
        public virtual ICollection<Loan>? Loans { get; set; }
        public virtual ICollection<Goal>? Goals { get; set; }
        public virtual ICollection<Saving>? Savings { get; set; }
    }
}
