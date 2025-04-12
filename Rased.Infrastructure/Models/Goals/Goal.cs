namespace Rased.Infrastructure.Models.Goals
{
    public enum GoalStatusEnum
    {
        InProgress = 1,
        Completed = 2,
        OnHold = 3,
        Canceled = 4
    }

    public class Goal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public GoalStatusEnum Status { get; set; } = GoalStatusEnum.InProgress;
        public DateTime StartedDate { get; set; }
        public DateTime DesiredDate { get; set; }
        public decimal StartedAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }

        public bool IsTemplate { get; set; }
        public string? Frequency { get; set; }   // N(Normal) - D(Daily) - M(Monthly) - Y(Yearly)
        public decimal? FrequencyAmount { get; set; }  // 3,000EGP
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parent Ids
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int? SubCatId { get; set; }

        // Navigation Properties
        public Wallet? Wallet { get; set; }
        public SharedWallet? SharedWallet { get; set; }
        public SubCategory? SubCategory { get; set; }
        public IEnumerable<GoalTransaction> GoalTransactions { get; set; } = new List<GoalTransaction>();
    }
}
