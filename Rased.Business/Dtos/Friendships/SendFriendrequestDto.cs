using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Friendships
{
    public class SendFriendRequestDto
    {
        [Required]
        public string ReceiverEmail { get; set; } = null!;
    }

    public class UpdateFriendRequestDto
    {
        [Required]
        public string SenderId { get; set; } = null!;
        public bool Status { get; set; }
    }

    public class RemoveFriendshipDto
    {
        [Required]
        public string ReceiverId { get; set; } = null!;
    }

    public class UserFriendDto
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Country { get; set; }
        public byte[]? ProfilePic { get; set; }
        public string? FriendshipStatus { get; set; }
        public int FriendsSince { get; set; }
    }
}
