using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface ITransactionRejectionService
    {
        Task<ApiResponse<string>> AddTransactionRejectionAsync(AddTransactionRejectionDto dto);
        Task<ApiResponse<ReadTransactionRejectionDto?>> GetRejectionByTransactionIdAsync(int transactionId);
        Task<ApiResponse<string>> UpdateRejectionReasonAsync(UpdateTransactionRejectionDto dto);
        Task<ApiResponse<string>> DeleteTransactionRejectionAsync(int rejectionId);
        Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllTransactionRejectionsAsync();
        Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForUserAndWalletAsync(string userId, int walletId);
        Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForUserAsync(string userId);
        Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForWalletAsync(int walletId);



    }

}
