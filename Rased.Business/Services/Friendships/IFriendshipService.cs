using Rased.Business.Dtos.Friendships;
using Rased.Business.Dtos.Response;

namespace Rased.Business.Services.Friendships
{
    public interface IFriendshipService
    {
        Task<ApiResponse<string>> SendFriendRequestAsync(SendFriendRequestDto model, string senderId);
        Task<ApiResponse<string>> UpdateFriendshipStatusAsync(UpdateFriendRequestDto model, string receiverId);
        Task<ApiResponse<string>> RemoveFriendshipAsync(RemoveFriendshipDto model, string senderId);
        Task<ApiResponse<IEnumerable<UserFriendDto>>> GetUserFriendsAsync(string userId);
        Task<bool> AreUsersFriendsAsync(string userId1, string userId2);
    }
}
