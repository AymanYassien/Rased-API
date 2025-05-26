using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Wallets;

namespace Rased.Business.Services.Wallets
{
    public interface IWalletService
    {
        Task<ApiResponse<IEnumerable<ReadWalletDto>>> GetAllWalletsAsync(string userId);
        Task<ApiResponse<ReadWalletDto>> GetWalletByIdAsync(int Id, string userId);
        Task<ApiResponse<string>> AddWalletAsync(RequestWalletDto model, string userId);
        Task<ApiResponse<string>> UpdateWalletAsync(RequestWalletDto model, int walletId, string userId);
        Task<ApiResponse<string>> RemoveWalletAsync(int Id, string userId);

        // Get Wallet Data Prts
        ApiResponse<WalletDataPartsDto> GetWalletDataParts();


        // In Transfer
        Task<bool> IsWalletOwnedByUserAsync(int walletId, string userId);
    }
}
