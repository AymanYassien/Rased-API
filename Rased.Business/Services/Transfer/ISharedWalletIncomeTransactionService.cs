using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
   public interface ISharedWalletIncomeTransactionService
    {
        Task<ApiResponse<string>> CreateSharedWalletIncomeTransactionAsync(AddSharedWalletIncomeTransactionDto dto);
        Task<ApiResponse<IQueryable<GetSharedWalletIncomeTransactionDto>>> GetSharedWalletIncomeTransactionByUserAndWalletAsync(string userId, int walletId);
        Task<ApiResponse<string>> UpdateSharedWalletIncomeTransactionAsync(int id, UpdateSharedWalletIncomeTransactionDto dto);
        Task<ApiResponse<string>> DeleteSharedWalletIncomeTransactionAsync(int id);





    }
}
