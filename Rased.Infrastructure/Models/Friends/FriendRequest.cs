using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Friends
{

    public class FriendRequest
    {
        public int RequestId { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public string? Status { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual RasedUser Sender { get; set; }
        public virtual RasedUser Receiver { get; set; }

    }
}