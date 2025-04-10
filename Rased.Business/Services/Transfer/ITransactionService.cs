using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public interface ITransactionService
    {
        Task<ApiResponse<IQueryable<ReadTransactionDto>>> GetAllTransactionsAsync();
        Task<ApiResponse<ReadTransactionDto?>> GetTransactionByIdAsync(int id);
        Task<ApiResponse<string>> AddTransactionAsync(AddTransactionDto dto);
        Task<ApiResponse<string>> UpdateTransactionAsync(UpdateTransactionDto dto);
        Task<ApiResponse<string>> DeleteTransactionAsync(int id);
        Task<ApiResponse<string>> ApproveTransactionAsync(TransactionApprovalDto dto);
        Task<ApiResponse<string>> RejectTransactionAsync(AddTransactionRejectionDto dto);
        Task<ApiResponse<List<ReadTransactionForSenderDto>>> GetTransactionsBySenderIdAsync(string userId, int senderWalletId);
        Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetTransactionsByReceiverIdAsync(string receiverId, int receiverWalletId);
        Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetReceivedTransactionsForUserAsync(string receiverId);
        Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetReceivedTransactionsForSharedWalletAsync(int sharedWalletId);
        Task<ApiResponse<IQueryable<ReadTransactionDto>>> GetTransactionsByWalletAndStatusAsync(int walletId, int statusId);
     }
}
