
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{

    public class FriendRequest
    {
        public int RequestId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public DateTime SentAt { get; set; }
        public int FriendRequestStatusId { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual RasedUser Sender { get; set; }
        public virtual RasedUser Receiver { get; set; }
        public virtual StaticFriendRequestStatusData StaticFriendRequestStatusData { get; set; }

    }
}