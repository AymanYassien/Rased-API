
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rased.Infrastructure
{
    public class Budget
    {

        public int BudgetId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Name { get; set; }
        public string? CategoryName { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal BudgetAmount { get; set; } // PlannedAmount
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public bool RolloverUnspent { get; set; }
        public int?  BudgetTypeId { get; set; }
        public int? DayOfMonth { get; set; }
        public int? DayOfWeek { get; set; }

        // Navigation properties
        public virtual Wallet? Wallet { get; set; }
        public virtual SharedWallet? SharedWallet { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual StaticBudgetTypesData StaticBudgetTypesData { get; set; }
        //public virtual ICollection<BudgetAlert> Alerts { get; set; }
    }
}


