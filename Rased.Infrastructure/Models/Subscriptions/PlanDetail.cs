namespace Rased.Infrastructure.Models.Subscriptions
{
    public class PlanDetail
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Type { get; set; }    // F(Feature) - L(Limit)
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int PlanId { get; set; }

        // Navigation Properties
        public Plan Plan { get; set; } = new Plan();
    }
}
