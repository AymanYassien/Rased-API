namespace Rased.Infrastructure.Models.Subscription
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public IEnumerable<Subscription> Subscriptions { get; set; } = new List<Subscription>();
        public IEnumerable<PlanDetail> PlanDetails { get; set; } = new List<PlanDetail>();
    }
}
