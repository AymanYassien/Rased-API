using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using Rased.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Rased.Business.Services.Transfer
{
    public class TransactionApprovalService : ITransactionApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransactionApprovalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovalsAsync()
        {
            var approvals = _unitOfWork.TransactionApprovals
                .GetAll()
                .ProjectTo<ReadTransactionApprovalDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionApprovalDto>>(approvals);
        }

        public async Task<ApiResponse<ReadTransactionApprovalDto?>> GetApprovalByIdAsync(int id)
        {
            var approval = await _unitOfWork.TransactionApprovals.GetByIdAsync(id);
            if (approval == null)
                return new ApiResponse<ReadTransactionApprovalDto?>("Approval not found");

            return new ApiResponse<ReadTransactionApprovalDto?>(_mapper.Map<ReadTransactionApprovalDto>(approval));
        }

        public async Task<ApiResponse<string>> AddTransactionApprovalAsync(AddTransactionApprovalDto dto)
        {
            var approval = new TransactionApproval
            {
                TransactionId = dto.TransactionId,
                ApproverId = dto.ApproverId,
                ApprovedAt = dto.ApprovedAt
            };

            await _unitOfWork.TransactionApprovals.AddAsync(approval);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "TransactionApproval added successfully");
        }

        public async Task<ApiResponse<string>> DeleteApprovalAsync(int id)
        {
            var approval = await _unitOfWork.TransactionApprovals.GetByIdAsync(id);
            if (approval == null)
                return new ApiResponse<string>("Approval not found");

            await _unitOfWork.TransactionApprovals.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Approval deleted successfully");
        }

        // Get Transaction Approval by TransactionId
        public async Task<ApiResponse<ReadTransactionApprovalDto?>> GetApprovalByTransactionIdAsync(int transactionId)
        {
            var approval = await _unitOfWork.TransactionApprovals
                .GetByIdAsync(transactionId);

            if (approval == null)
                return new ApiResponse<ReadTransactionApprovalDto?>("Approval not found");

            var approvalDto = _mapper.Map<ReadTransactionApprovalDto>(approval);
            return new ApiResponse<ReadTransactionApprovalDto?>(approvalDto);
        }

        // Get all transaction approvals for a specific user and wallet
        public async Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForUserAndWalletAsync(string userId, int walletId)
        {
            var approvedTransactions = _unitOfWork.TransactionApprovals
                .GetAll()
                .Where(ta => ta.ApproverId == userId &&
                             (ta.Transaction.SenderWalletId == walletId || ta.Transaction.ReceiverWalletId == walletId)) // Filter by userId and walletId
                .ProjectTo<ReadTransactionApprovalDto>(_mapper.ConfigurationProvider); // Mapping to DTO

            return new ApiResponse<IQueryable<ReadTransactionApprovalDto>>(approvedTransactions);
        }

        // Get all transaction approvals for a specific user
        public async Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForUserAsync(string userId)
        {
            var approvedTransactions = _unitOfWork.TransactionApprovals
                .GetAll()
                .Where(ta => ta.ApproverId == userId) // Filter by userId
                .ProjectTo<ReadTransactionApprovalDto>(_mapper.ConfigurationProvider); 

            return new ApiResponse<IQueryable<ReadTransactionApprovalDto>>(approvedTransactions);
        }

        // Get all transaction approvals for a specific wallet
        public async Task<ApiResponse<IQueryable<ReadTransactionApprovalDto>>> GetAllApprovedTransactionsForWalletAsync(int walletId)
        {
            var approvedTransactions = _unitOfWork.TransactionApprovals
                .GetAll()
                .Where(ta => ta.Transaction.SenderWalletId == walletId || ta.Transaction.ReceiverWalletId == walletId) // Filter by walletId
                .ProjectTo<ReadTransactionApprovalDto>(_mapper.ConfigurationProvider); // Mapping to DTO

            return new ApiResponse<IQueryable<ReadTransactionApprovalDto>>(approvedTransactions);
        }
    }

}
