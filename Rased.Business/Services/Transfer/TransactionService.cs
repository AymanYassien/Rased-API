using AutoMapper;
using AutoMapper.QueryableExtensions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.ExpenseService;
using Rased.Business.Services.Friendships;
using Rased.Business.Services.SharedWallets;
using Rased.Infrastructure;
using Rased.Infrastructure.Models.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Rased.Business.Services.Transfer
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExpenseService _expenseService;
        private readonly IFriendshipService _friendshipService;
        private readonly ISharedWalletService _sharedWalletService;

        public TransactionService(IUnitOfWork unitOfWork, IMapper mapper , IExpenseService expenseService , IFriendshipService friendshipService
            , ISharedWalletService sharedWalletService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _expenseService = expenseService;
            _friendshipService = friendshipService;
            _sharedWalletService = sharedWalletService;
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionDto>>> GetAllTransactionsAsync()
        {
            var transactions = _unitOfWork.Transactions
                .GetAll()
                .ProjectTo<ReadTransactionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionDto>>(transactions);
        }

        public async Task<ApiResponse<ReadTransactionDto?>> GetTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
                return new ApiResponse<ReadTransactionDto?>("Transaction not found");

            return new ApiResponse<ReadTransactionDto?>(_mapper.Map<ReadTransactionDto>(transaction));
        }

        public async Task<ApiResponse<string>> AddTransactionAsync(AddTransactionDto dto)
        {
            try
            {
                // Check if it's a friend transaction
                if (dto.ReceiverTypeId == 1) // 1 means "Friend"
                {
                    bool areFriends = await _friendshipService.AreUsersFriendsAsync(dto.SenderId, dto.ReceiverId);
                    if (!areFriends)
                    {
                        return new ApiResponse<string>("Transaction failed. Sender and receiver are not friends.");
                    }
                }

                // Check if it's a Shared Wallet transaction (ReceiverTypeId == 2)
                if (dto.ReceiverTypeId == 2) // 2 means "Shared Wallet"
                {
                    if (!dto.ReceiverWalletId.HasValue)
                    {
                        return new ApiResponse<string>("Transaction failed. ReceiverWalletId is required for shared wallet transactions.");
                    }

                    var isUserInSharedWallet = await _sharedWalletService.IsUserInSharedWalletAsync(dto.SenderId, dto.ReceiverWalletId.Value);
                    if (!isUserInSharedWallet.Succeeded || !isUserInSharedWallet.Data)
                    {
                        return new ApiResponse<string>("Transaction failed. Sender is not a member of the shared wallet.");
                    }
                }
                var transaction = _mapper.Map<Infrastructure.Transaction>(dto);
                transaction.CreatedAt = DateTime.UtcNow;
                transaction.TransactionStatusId = 1; // Pending

                await _unitOfWork.Transactions.AddAsync(transaction);
                await _unitOfWork.CommitChangesAsync();
                

                var expense = await _expenseService.AddUserExpense(_mapper.Map<AddExpenseDto>(dto));
                await _unitOfWork.CommitChangesAsync();

                //Add ExpenseTransactionReord
                var expenseTransactionRecord = new ExpenseTransactionRecord
                {
                    //ExpenseId = expense.Da,
                    TransactionId = transaction.TransactionId,
                    CreatedAt = DateTime.UtcNow
                };
                await _unitOfWork.ExpenseTransactionRecords.AddAsync(expenseTransactionRecord);
                await _unitOfWork.CommitChangesAsync();

                return new ApiResponse<string>(null, "Transaction added successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }
        
        
        

        public async Task<ApiResponse<string>> UpdateTransactionAsync(UpdateTransactionDto dto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(dto.TransactionId);
            if (transaction == null)
                return new ApiResponse<string>("Transaction not found");

            _mapper.Map(dto, transaction);
            transaction.UpdatedAt = DateTime.UtcNow;
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction updated successfully");
        }

        public async Task<ApiResponse<string>> DeleteTransactionAsync(int id)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(id);
            if (transaction == null)
                return new ApiResponse<string>("Transaction not found");

            await _unitOfWork.Transactions.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction deleted successfully");
        }

        public async Task<ApiResponse<string>> ApproveTransactionAsync(TransactionApprovalDto dto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(dto.TransactionId);
            if (transaction == null)
                return new ApiResponse<string>("Transaction not found");

            if (transaction.ReceiverTypeId == 1)
            {
                if (transaction.ReceiverId != dto.ApproverId)
                    return new ApiResponse<string>("Only the receiver can approve this transaction");

                // هنا لازم المستخدم يكون بعث ID المحفظة اللي عايز يستقبل عليها
                if (dto.ReceiverWalletId == null)
                    return new ApiResponse<string>("Please select a personal wallet to receive the transaction");

                transaction.ReceiverWalletId = dto.ReceiverWalletId; // نثبت المحفظة اللي اختارها
            }
            else if (transaction.ReceiverTypeId == 2) // Shared Wallet
            {
                if (transaction.ReceiverWalletId == null)
                    return new ApiResponse<string>("Transaction does not have a valid shared wallet");

                var isAdminResult = await _sharedWalletService.IsUserAdminOfSharedWalletAsync(dto.ApproverId, transaction.ReceiverWalletId.Value);
                if (!isAdminResult.Succeeded || !isAdminResult.Data)
                {
                    return new ApiResponse<string>("Only an admin of the shared wallet can approve this transaction");
                }

            }
            else
            {
                return new ApiResponse<string>("Invalid receiver type");
            }

            var approval = new TransactionApproval
            {
                TransactionId = dto.TransactionId,
                ApproverId = dto.ApproverId,
                ApprovedAt = DateTime.UtcNow
            };
            await _unitOfWork.TransactionApprovals.AddAsync(approval);

            var income = new Income
            {
                WalletId = transaction.ReceiverTypeId == 1 ? transaction.ReceiverWalletId : null,
                SharedWalletId = transaction.ReceiverTypeId == 2 ? transaction.ReceiverWalletId : null,
                Amount = transaction.Amount,
                Title = "Transaction Income",
                Description = transaction.Description,
                IsAutomated = true
            };
            //await _unitOfWork.Incomes.AddAsync(income);
            await _unitOfWork.CommitChangesAsync();

            if (transaction.ReceiverTypeId == 1)
            {
                var personalRecord = new PersonalIncomeTrasactionRecord
                {
                    IncomeId = income.IncomeId,
                    TransactionId = transaction.TransactionId,
                    CreatedAt = DateTime.UtcNow,
                    ApprovalId = approval.ApprovalId
                };
                await _unitOfWork.PersonalIncomeTrasactionRecords.AddAsync(personalRecord);
            }
            else
            {
                var sharedRecord = new SharedWalletIncomeTransaction
                {
                    IncomeId = income.IncomeId,
                    TransactionId = transaction.TransactionId,
                    CreatedAt = DateTime.UtcNow,
                    ApprovalId = approval.ApprovalId
                };
                await _unitOfWork.SharedWalletIncomeTransactions.AddAsync(sharedRecord);
            }

            transaction.TransactionStatusId = 2; // Approved
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction approved and income generated");
        }


        public async Task<ApiResponse<string>> RejectTransactionAsync(AddTransactionRejectionDto dto)
        {
            var transaction = await _unitOfWork.Transactions.GetByIdAsync(dto.TransactionId);
            if (transaction == null)
                return new ApiResponse<string>("Transaction not found");

            if (transaction.ReceiverId != dto.RejectedBy)
                return new ApiResponse<string>("Only the receiver can reject this transaction");

            var rejection = new TransactionRejection
            {
                TransactionId = dto.TransactionId,
                RejectedById = dto.RejectedBy,
                RejectedAt = DateTime.UtcNow,
                RejectionReason = dto.RejectionReason
            };

            await _unitOfWork.TransactionRejections.AddAsync(rejection);
            transaction.TransactionStatusId = 3; // Rejected
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Transaction rejected successfully");
        }

        public async Task<ApiResponse<List<ReadTransactionForSenderDto>>> GetTransactionsBySenderIdAsync(string userId, int senderWalletId)
        {
            
            var transactions = await _unitOfWork.Transactions
                .FindAsync(t => t.SenderId == userId && t.SenderWalletId == senderWalletId); 

         
            var mapped = _mapper.Map<List<ReadTransactionForSenderDto>>(transactions);
            return new ApiResponse<List<ReadTransactionForSenderDto>>(mapped);
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetTransactionsByReceiverIdAsync( string receiverId, int receiverWalletId)
        {
            var transactionsQuery = _unitOfWork.Transactions.GetAll()
                .Where(t => t.ReceiverWalletId == receiverWalletId && t.ReceiverId == receiverId)
                .Where(t => t.TransactionStatusId != 4); 

            var mapped = transactionsQuery
                .ProjectTo<ReadTransactionForReceiverDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionForReceiverDto>>(mapped);
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetReceivedTransactionsForUserAsync(string receiverId)
        {
            var transactions = _unitOfWork.Transactions.GetAll()
                .Where(t => t.ReceiverId == receiverId && t.ReceiverTypeId == 1) //To  Personal user
                .Where(t => t.TransactionStatusId == 1); // Pending

            var mapped = transactions
                .ProjectTo<ReadTransactionForReceiverDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionForReceiverDto>>(mapped);
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionForReceiverDto>>> GetReceivedTransactionsForSharedWalletAsync(int sharedWalletId)
        {
            var transactions = _unitOfWork.Transactions.GetAll()
                .Where(t => t.ReceiverWalletId == sharedWalletId && t.ReceiverTypeId == 2)
                .Where(t => t.TransactionStatusId == 1); // Pending

            var mapped = transactions
                .ProjectTo<ReadTransactionForReceiverDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionForReceiverDto>>(mapped);
        }

        public async Task<ApiResponse<IQueryable<ReadTransactionDto>>> GetTransactionsByWalletAndStatusAsync(int walletId, int statusId)
        {
            var transactions = _unitOfWork.Transactions.GetAll()
                .Where(t => t.SenderWalletId == walletId || t.ReceiverWalletId == walletId)
                .Where(t => t.TransactionStatusId == statusId);

            var mappedTransactions = transactions
                .ProjectTo<ReadTransactionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadTransactionDto>>(mappedTransactions);
        }






    }

}
