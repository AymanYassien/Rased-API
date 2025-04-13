using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SharedWallets;

namespace Rased.Business.Services.SharedWallets
{
    public interface ISharedWalletsService
    {
        Task<ApiResponse<IEnumerable<ReadSharedWalletDto>>> ReadAllAsync(string userId);
        Task<ApiResponse<ReadSharedWalletDto>> ReadSingleAsync(int id, string userId);
        Task<ApiResponse<string>> CreateAsync(SharedWalletDto model, string userId);
        Task<ApiResponse<string>> UpdateAsync(SharedWalletDto model, int walletId, string userId);
        Task<ApiResponse<string>> RemoveAsync(int id, string userId);

        // Invitations
        //Task<ApiResponse<string>> SendInviteAsync(string userName);
        //Task<ApiResponse<string>> UpdateInviteStatusAsync(string userName);
    }
}
