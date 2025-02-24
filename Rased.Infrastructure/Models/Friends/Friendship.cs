using Rased.Infrastructure.Helpers.Utilities;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Friends
{
    public class Friendship
    {
        public int FriendshipId { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Nickname1 { get; set; }

        public string? Nickname2 { get; set; }
        public string? Status { get; set; }

        // Navigation properties
        public virtual RasedUser User1Profile { get; set; }

        public virtual RasedUser User2Profile { get; set; }

    }
}