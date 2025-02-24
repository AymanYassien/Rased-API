using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.Categories;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure.Models.Expenses
{

    public class ExpenseTemplate
    {
        public int TemplateId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int AutomationRuleId { get; set; }
        public string Name { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal Amount { get; set; }
        public bool IsNeedApprovalWhenAutoAdd { get; set; }
        public string Description { get; set; }
        public string? DefaultPayment { get; set; }

        // Navigation properties
        public virtual Wallet Wallet { get; set; }
        public virtual SharedWallet SharedWallet { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual AutomationRule AutomationRule { get; set; }
    }
}