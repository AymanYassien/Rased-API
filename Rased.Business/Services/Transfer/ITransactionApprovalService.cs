using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface ITransactionApprovalService
    {
        Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovalsAsync();
        Task<ApiResponse<ReadTransactionApprovalDto?>> GetApprovalByIdAsync(int id);
        Task<ApiResponse<string>> AddTransactionApprovalAsync(AddTransactionApprovalDto dto);
        Task<ApiResponse<string>> DeleteApprovalAsync(int id);
        Task<ApiResponse<ReadTransactionApprovalDto?>> GetApprovalByTransactionIdAsync(int transactionId);
        Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForUserAndWalletAsync(string userId, int walletId);
        Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForUserAsync(string userId);
        Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForWalletAsync(int walletId);
    }
}
