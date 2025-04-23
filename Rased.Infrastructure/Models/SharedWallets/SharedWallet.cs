using Rased.Infrastructure.Models.Bills;
using Rased.Infrastructure.Models.Debts;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.SharedWallets
{
    public class SharedWallet
    {
        public int SharedWalletId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public int WalletStatusId { get; set; }
        public int ColorTypeId { get; set; }
        public int CurrencyId { get; set; }

        // Navigation Properties
        public virtual Currency Currency { get; set; } = new Currency();
        public virtual StaticWalletStatusData StaticWalletStatusData { get; set; } = new StaticWalletStatusData();
        public virtual StaticColorTypeData StaticColorTypeData { get; set; } = new StaticColorTypeData();

        // Has Many Members
        public virtual ICollection<RasedUser> Members { get; set; } = new List<RasedUser>();
        // Has SharedWallet Members
        public virtual ICollection<SharedWalletMembers> SWMembers { get; set; } = new List<SharedWalletMembers>();
        // Has Many invitations
        public virtual ICollection<SWInvitation> SWInvitations { get; set; } = new List<SWInvitation>();

        public virtual ICollection<Income>? Incomes { get; set; }
        public virtual ICollection<IncomeTemplate>? IncomeTemplates { get; set; }
        public virtual ICollection<Expense>? Expenses { get; set; }
        public virtual ICollection<ExpenseTemplate>? ExpensesTemplates { get; set; }
        public virtual ICollection<Budget>? Budgets { get; set; }
        public virtual ICollection<Loan>? Loans { get; set; }
        public virtual ICollection<Goal>? Goals { get; set; }
        public virtual ICollection<Saving>? Savings { get; set; }
        public virtual ICollection<Transaction> SentTransactions { get; set; }
        public virtual ICollection<Transaction> ReceivedTransactions { get; set; }
        public virtual ICollection<BillDraft>? BillDrafts { get; set; }


    }
}