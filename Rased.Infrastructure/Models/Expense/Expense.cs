
using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure
{

    public class Expense
    {
        public int ExpenseId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public DateTime Date { get; set; }
        public int? PaymentMethodId { get; set; }
        public bool IsAutomated { get; set; }
        public int? RelatedBudgetId { get; set; }
        public int? TemplateId { get; set; }

        // Navigation properties
        public virtual Wallet Wallet { get; set; }
        public virtual SharedWallet SharedWallet { get; set; }
        public virtual Category Category { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Budget RelatedBudget { get; set; }
        public virtual ExpenseTemplate Template { get; set; }
        public virtual StaticPaymentMethodsData StaticPaymentMethodsData { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }

    }

   
}