using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Wallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Wallets
{
    public interface IWalletService
    {
        Task<ApiResponse<IEnumerable<ReadWalletDto>>> GetAllWalletsAsync(string curUserId);
        //Task<ApiResponse<ReadWalletDto>> GetWalletByIdAsync(int Id);
        Task<ApiResponse<string>> AddWalletAsync(RequestWalletDto model, string userId);
        //Task<ApiResponse<string>> UpdateWalletAsync(RequestWalletDto model);
        //Task<ApiResponse<string>> RemoveWalletAsync(int Id);
    }
}
