using Rased.Infrastructure.Models.Budgets;
using Rased.Infrastructure.Models.Categories;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure.Models.Expenses
{

    public class Expense
    {
        public int ExpenseId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int? SubCategoryId { get; set; }
        public DateTime Date { get; set; }
        public string? PaymentMethod { get; set; }
        public bool IsAutomated { get; set; }
        public int? RelatedBudgetId { get; set; }
        public int? TemplateId { get; set; }

        // Navigation properties
        public virtual Wallet Wallet { get; set; }
        public virtual SharedWallet SharedWallet { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        public virtual Budget RelatedBudget { get; set; }
        public virtual ExpenseTemplate Template { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }
    }
}