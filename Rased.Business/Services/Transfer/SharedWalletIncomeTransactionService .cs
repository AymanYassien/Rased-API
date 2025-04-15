using Rased.Infrastructure.UnitsOfWork;
using Rased.Infrastructure;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Dtos.Response;
using System.Linq;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public class SharedWalletIncomeTransactionService : ISharedWalletIncomeTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SharedWalletIncomeTransactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CreateSharedWalletIncomeTransactionAsync(AddSharedWalletIncomeTransactionDto dto)
        {
            var transaction = new SharedWalletIncomeTransaction
            {
                TransactionId = dto.TransactionId,
                IncomeId = dto.IncomeId,
                ApprovalId = dto.ApprovalId,
                CreatedAt = dto.CreatedAt
            };

            await _unitOfWork.SharedWalletIncomeTransactions.AddAsync(transaction);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "SharedWalletIncomeTransaction created successfully");
        }

        public async Task<ApiResponse<IQueryable<GetSharedWalletIncomeTransactionDto>>> GetSharedWalletIncomeTransactionByUserAndWalletAsync(string userId, int walletId)
        {
            var records = _unitOfWork.SharedWalletIncomeTransactions
                .FindAll(r => r.Transaction.SenderId == userId && r.Transaction.SenderWalletId == walletId && !r.isDeleted);

            var recordsDto = records.Select(r => new GetSharedWalletIncomeTransactionDto
            {
                SharedWalletIncomeTransactionId = r.SharedWalletIncomeTransactionId,
                TransactionId = r.TransactionId,
                IncomeId = r.IncomeId,
                ApprovalId = r.ApprovalId,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return new ApiResponse<IQueryable<GetSharedWalletIncomeTransactionDto>>(recordsDto);
        }

        public async Task<ApiResponse<string>> UpdateSharedWalletIncomeTransactionAsync(int id, UpdateSharedWalletIncomeTransactionDto dto)
        {
            var transaction = await _unitOfWork.SharedWalletIncomeTransactions.GetByIdAsync(id);
            if (transaction == null || transaction.isDeleted)
            {
                return new ApiResponse<string>("Transaction not found or already deleted");
            }

        

            if (dto.ApprovalId.HasValue)
            {
                transaction.ApprovalId = dto.ApprovalId.Value;
            }

            if (dto.IncomeSpecificData != null)
            {
                transaction.IncomeSpecificData = dto.IncomeSpecificData;
            }

            transaction.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "SharedWalletIncomeTransaction updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteSharedWalletIncomeTransactionAsync(int id)
        {
            var transaction = await _unitOfWork.SharedWalletIncomeTransactions.GetByIdAsync(id);
            if (transaction == null || transaction.isDeleted)
            {
                return new ApiResponse<string>("Transaction not found or already deleted");
            }

            transaction.isDeleted = true;
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "SharedWalletIncomeTransaction deleted successfully");
        }
    }
}
