using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SharedWallets;

namespace Rased.Business.Services.SharedWallets
{
    public interface ISharedWalletService
    {
        ApiResponse<IEnumerable<ReadSharedWalletDto>> ReadAllAsync(string userId);
        ApiResponse<ReadSharedWalletDto> ReadSingleAsync(int id, string userId);
        Task<ApiResponse<string>> CreateAsync(SharedWalletDto model, string userId);
        Task<ApiResponse<string>> UpdateAsync(SharedWalletDto model, int walletId, string userId);
        Task<ApiResponse<string>> RemoveAsync(int id, string userId);

        // Invitations
        Task<ApiResponse<string>> SendInviteAsync(SWInvitationDto model, string senderId);
        Task<ApiResponse<string>> UpdateInviteStatusAsync(UpdateInvitationDto model, string receiverId);

        //// Members Control
        Task<ApiResponse<string>> UpdateMemberAccessLevelAsync(UpdateMemberRoleDto model, string userId); // Only for Owner
        Task<ApiResponse<string>> RemoveMemberAsync(RemoveMemberDto model, string userId); // Only for Owner and SuperVisor


        Task<ApiResponse<bool>> IsUserInSharedWalletAsync(string userId, int? sharedWalletId);
        Task<ApiResponse<bool>> IsUserAdminOfSharedWalletAsync(string userId, int sharedWalletId);
   
    }
}
