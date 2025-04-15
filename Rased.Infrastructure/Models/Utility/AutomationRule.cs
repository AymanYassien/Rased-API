namespace Rased.Infrastructure
{
    public class AutomationRule
    {

        public int AutomationRuleId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DayOfMonth { get; set; }
        public int? DayOfWeek { get; set; }
        public int? TriggerTypeId { get; set; }
        
        //public virtual Wallet Wallet { get; set; }
        //public virtual SharedWallet SharedWallet { get; set; } // Assumed class
        public virtual IncomeTemplate IncomeTemplate { get; set; }
        public virtual ExpenseTemplate ExpenseTemplate { get; set; }
        public virtual StaticTriggerTypeData StaticTriggerTypeData { get; set; } 
    }

}

    