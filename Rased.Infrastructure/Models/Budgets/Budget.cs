using Rased.Infrastructure.Models.Categories;
using Rased.Infrastructure.Models.Expenses;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure.Models.Budgets
{
    public class Budget
    {

        public int BudgetId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal PlannedAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool RolloverUnspent { get; set; }
        public string? Status { get; set; }
        public int? DayOfMonth { get; set; }
        public int? DayOfWeek { get; set; }

        // Navigation properties
        public virtual Wallet Wallet { get; set; } = new Wallet();
        public virtual SharedWallet SharedWallet { get; set; } = new SharedWallet();
        public virtual SubCategory SubCategory { get; set; } = new SubCategory();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        //public virtual ICollection<BudgetAlert> Alerts { get; set; }
    }
}


