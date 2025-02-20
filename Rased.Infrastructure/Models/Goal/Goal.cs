namespace Rased.Infrastructure.Models.Goal
{
    public class Goal
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }   // C(Completed) - P(Progressing)
        public DateTime StartedDate { get; set; }
        public DateTime DesiredDate { get; set; }
        public decimal StartedAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }

        public bool IsTemplate { get; set; }
        public string? Frequency { get; set; }   // N(Normal) - D(Daily) - M(Monthly) - Y(Yearly)
        public decimal FrequencyAmount { get; set; }  // 3,000EGP
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parent Ids
        public int WalletId { get; set; }
        public int GoalCatId { get; set; }

        // Navigation Properties
        //public Wallet Wallet { get; set; } = new Wallet();
        public GoalCategory GoalCategory { get; set; } = new GoalCategory();
        public IEnumerable<GoalTransaction> GoalTransactions { get; set; } = new List<GoalTransaction>();
    }
}
