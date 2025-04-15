using AutoMapper;
using AutoMapper.QueryableExtensions;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure.Models.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Transfer
{
    public class TransactionRejectionService : ITransactionRejectionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionRejectionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // Add Transaction Rejection
        public async Task<ApiResponse<string>> AddTransactionRejectionAsync(AddTransactionRejectionDto dto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(dto.TransactionId);
            if (transaction == null)
                return new ApiResponse<string>("Transaction not found");

            var rejection = new TransactionRejection
            {
                TransactionId = dto.TransactionId,
                RejectedById = dto.RejectedBy,
                RejectedAt = DateTime.UtcNow,
                RejectionReason = dto.RejectionReason,
                
            };

            await _unitOfWork.TransactionRejections.AddAsync(rejection);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction rejected successfully");
        }

        // Get Transaction Rejection by TransactionId
        public async Task<ApiResponse<ReadTransactionRejectionDto?>> GetRejectionByTransactionIdAsync(int transactionId)
        {
            var rejection = await _unitOfWork.TransactionRejections
                .GetByIdAsync(transactionId);

            if (rejection == null)
                return new ApiResponse<ReadTransactionRejectionDto?>("Rejection not found");

            var rejectionDto = _mapper.Map<ReadTransactionRejectionDto>(rejection);
            return new ApiResponse<ReadTransactionRejectionDto?>(rejectionDto);
        }

        // Update Transaction Rejection Reason
        public async Task<ApiResponse<string>> UpdateRejectionReasonAsync(UpdateTransactionRejectionDto dto)
        {
            var rejection = await _unitOfWork.TransactionRejections.GetByIdAsync(dto.RejectionId);
            if (rejection == null)
                return new ApiResponse<string>("Rejection record not found");

            rejection.RejectionReason = dto.NewRejectionReason;
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Rejection reason updated successfully");
        }

        // Delete Transaction Rejection
        public async Task<ApiResponse<string>> DeleteTransactionRejectionAsync(int rejectionId)
        {
            var rejection = await _unitOfWork.Transactions.GetByIdAsync(rejectionId);
            if (rejection == null)
                return new ApiResponse<string>("Rejection record not found");

            await _unitOfWork.Transactions.DeleteByIdAsync(rejectionId);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Rejection record deleted successfully");
        }

        //Get All Transaction Rejection
        public async Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllTransactionRejectionsAsync()
        {
            var rejections = _unitOfWork.TransactionRejections.GetAll()
                .ProjectTo<ReadTransactionRejectionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionRejectionDto>>(rejections);
        }

        // Get all transaction rejections for a specific user and wallet
        public async Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForUserAndWalletAsync(string userId, int walletId)
        {
            var rejectedTransactions = _unitOfWork.TransactionRejections
                .GetAll()
                .Where(tr => tr.RejectedById == userId &&
                             (tr.Transaction.SenderWalletId == walletId || tr.Transaction.ReceiverWalletId == walletId)) // Filter by userId and walletId
                .ProjectTo<ReadTransactionRejectionDto>(_mapper.ConfigurationProvider); // Mapping to DTO

            return new ApiResponse<IQueryable<ReadTransactionRejectionDto>>(rejectedTransactions);
        }

        // Get all transaction rejections for a specific user
        public async Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForUserAsync(string userId)
        {
            var rejectedTransactions = _unitOfWork.TransactionRejections
                .GetAll()
                .Where(tr => tr.RejectedById == userId) // Filter by userId
                .ProjectTo<ReadTransactionRejectionDto>(_mapper.ConfigurationProvider); // Mapping to DTO

            return new ApiResponse<IQueryable<ReadTransactionRejectionDto>>(rejectedTransactions);
        }

        // Get all transaction rejections for a specific wallet
        public async Task<ApiResponse<IQueryable<ReadTransactionRejectionDto>>> GetAllRejectedTransactionsForWalletAsync(int walletId)
        {
            var rejectedTransactions = _unitOfWork.TransactionRejections
                .GetAll()
                .Where(tr => tr.Transaction.SenderWalletId == walletId || tr.Transaction.ReceiverWalletId == walletId) // Filter by walletId
                .ProjectTo<ReadTransactionRejectionDto>(_mapper.ConfigurationProvider); // Mapping to DTO

            return new ApiResponse<IQueryable<ReadTransactionRejectionDto>>(rejectedTransactions);
        }

    }
}