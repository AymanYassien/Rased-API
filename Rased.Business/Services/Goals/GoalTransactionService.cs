using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Goals
{
    public class GoalTransactionService : IGoalTransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoalTransactionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetAllGoalsTransactionAsync()
        {
            var transactions = _unitOfWork.GoalTransactionRepository.GetAll()
                .ProjectTo<ReadGoalTransactionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalTransactionDto>>(transactions);
        }

        public async Task<ApiResponse<ReadGoalTransactionDto?>> GetGoalTransactionByIdAsync(int id)
        {
            var transaction = await _unitOfWork.GoalTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return new ApiResponse<ReadGoalTransactionDto?>("Goal Transaction not found.");

            return new ApiResponse<ReadGoalTransactionDto?>(_mapper.Map<ReadGoalTransactionDto>(transaction));
        }

        public async Task<ApiResponse<string>> AddGoalTransactionAsync(AddGoalTransactionDto addGoalTransactionDto)
        {
            await _unitOfWork.GoalTransactionRepository.AddAsync(_mapper.Map<GoalTransaction>(addGoalTransactionDto));
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal Transaction added successfully.");
        }

        public async Task<ApiResponse<string>> UpdateGoalTransactionAsync(UpdateGoalTransactionDto updateGoalTransactionDto)
        {
            var transaction = await _unitOfWork.GoalTransactionRepository.GetByIdAsync(updateGoalTransactionDto.Id);
            if (transaction == null)
                return new ApiResponse<string>("Goal Transaction not found.");

            _mapper.Map(updateGoalTransactionDto, transaction);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal Transaction updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteGoalTransactionAsync(int id)
        {
            var transaction = await _unitOfWork.GoalTransactionRepository.GetByIdAsync(id);
            if (transaction == null)
                return new ApiResponse<string>("Goal Transaction not found.");

            await _unitOfWork.GoalTransactionRepository.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal Transaction deleted successfully.");
        }

        public async Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetTransactionsByGoalIdAsync(int goalId)
        {
          
            bool goalExists = await _unitOfWork.GoalRepository.AnyAsync(g => g.Id == goalId);
            if (!goalExists)
                return new ApiResponse<IQueryable<ReadGoalTransactionDto>>("Goal not found.");

          
            var transactions = _unitOfWork.GoalTransactionRepository
                .GetByCondition(t => t.GoalId == goalId)
                .AsNoTracking() 
                .ProjectTo<ReadGoalTransactionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalTransactionDto>>(transactions);
        }

        public async Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetTransactionsByWalletIdAsync(int walletId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId != null);
            if (!walletExists)
                return new ApiResponse<IQueryable<ReadGoalTransactionDto>>("Wallet not found or not associated with a user.");

            var transactions = _unitOfWork.GoalTransactionRepository
                .GetByCondition(t => t.Goal.WalletId == walletId)
                .ProjectTo<ReadGoalTransactionDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalTransactionDto>>(transactions);
        }

        public async Task<ApiResponse<bool>> IsGoalCompletedAsync(int goalId)
        {
            var goal = await _unitOfWork.GoalRepository.FindAsync(g => g.Id == goalId);
            if (goal == null)
                return new ApiResponse<bool>("Goal not found.");

           
            decimal totalSavedAmount = await _unitOfWork.GoalTransactionRepository
                .GetByCondition(t => t.GoalId == goalId)
                .SumAsync(t => t.InsertedAmount);

        
            bool isCompleted = totalSavedAmount >= goal.TargetAmount;

            return new ApiResponse<bool>(isCompleted);
        }

        public async Task<ApiResponse<decimal>> GetTotalSavedAmountByDateRangeAsync(int goalId, DateTime startDate, DateTime endDate)
        {
           
            bool goalExists = await _unitOfWork.GoalRepository.AnyAsync(g => g.Id == goalId);
            if (!goalExists)
                return new ApiResponse<decimal>("Goal not found.");

           
            decimal totalSavedAmount = await _unitOfWork.GoalTransactionRepository
                .GetByCondition(t => t.GoalId == goalId && t.InsertedDate >= startDate && t.InsertedDate <= endDate)
                .SumAsync(t => t.InsertedAmount);

            return new ApiResponse<decimal>(totalSavedAmount);
        }





    }

}
