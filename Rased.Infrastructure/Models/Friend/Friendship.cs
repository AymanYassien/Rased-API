using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{
    public class Friendship
    {
        public int Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string Status { get; set; } = null!; // Acc, Pend
        public DateTime SentAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual RasedUser Sender { get; set; } = null!;
        public virtual RasedUser Receiver { get; set; } = null!;
    }
}