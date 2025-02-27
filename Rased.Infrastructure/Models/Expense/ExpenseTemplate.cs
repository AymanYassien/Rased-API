

namespace Rased.Infrastructure
{

    public class ExpenseTemplate
    {
        public int TemplateId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int AutomationRuleId { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public decimal Amount { get; set; }
        public bool IsNeedApprovalWhenAutoAdd { get; set; }
        public string Description { get; set; }
        public int? PaymentMethodId { get; set; }

        // Navigation properties
        public virtual Wallet Wallet { get; set; }
        public virtual SharedWallet SharedWallet { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual StaticPaymentMethodsData StaticPaymentMethodsData { get; set; }
        public virtual AutomationRule AutomationRule { get; set; }
    }
}