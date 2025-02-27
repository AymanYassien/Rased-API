namespace Rased.Infrastructure.Models.Goals
{
    public class GoalTransaction
    {
        public int Id { get; set; }
        public decimal InsertedAmount { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int GoalId { get; set; }

        // Navigation Properties
        public Goal Goal { get; set; } = new Goal();
    }
}
