namespace Rased.Infrastructure
{
    public class Income
    {
        public int IncomeId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Title { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public int IncomeSourceTypeId { get; set; }
        public int? IncomeTemplateId { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsAutomated { get; set; }
        public int? SubCategoryId { get; set; }


        public virtual Wallet? Wallet { get; set; }
        public virtual SharedWallet? SharedWallet { get; set; }
        public virtual SubCategory? SubCategory { get; set; }
        public virtual StaticIncomeSourceTypeData IncomeSourceType { get; set; } 
        //public virtual IncomeTemplate IncomeTemplate { get; set; } 
    }
    
}
