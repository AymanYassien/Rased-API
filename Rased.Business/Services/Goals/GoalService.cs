using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Goals
{

    public class GoalService : IGoalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoalService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IQueryable<ReadGoalDto>>> GetAllGoalsAsync()
        {
            var goals = _unitOfWork.GoalRepository.GetAll()
                .ProjectTo<ReadGoalDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalDto>>(goals);
        }

        public async Task<ApiResponse<ReadGoalDto>> GetGoalByIdAsync(int id)
        {
            var goal = await _unitOfWork.GoalRepository.GetByIdAsync(id);
            if (goal == null)
                return new ApiResponse<ReadGoalDto>("Goal not found.");

            return new ApiResponse<ReadGoalDto>(_mapper.Map<ReadGoalDto>(goal));
        }

        public async Task<ApiResponse<string>> AddGoalAsync(AddGoalDto addGoalDto)
        {
            var goal = _mapper.Map<Goal>(addGoalDto);
            goal.CurrentAmount = goal.StartedAmount;

            await _unitOfWork.GoalRepository.AddAsync(goal);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal added successfully.");
        }


        public async Task<ApiResponse<string>> UpdateGoalAsync(UpdateGoalDto updateGoalDto)
        {
            var goal = await _unitOfWork.GoalRepository.GetByIdAsync(updateGoalDto.Id);
            if (goal == null)
                return new ApiResponse<string>("Goal not found.");

            _mapper.Map(updateGoalDto, goal);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteGoalAsync(int id)
        {
            var goal = await _unitOfWork.GoalRepository.GetByIdAsync(id);
            if (goal == null)
                return new ApiResponse<string>("Goal not found.");

            await _unitOfWork.GoalRepository.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Goal deleted successfully.");
        }


        public async Task<ApiResponse<IQueryable<ReadGoalDto>>> GetGoalsByStatusAsync(GoalStatusEnum status)
        {
            var goals = _unitOfWork.GoalRepository
                .GetByCondition(g => g.Status == status) 
                .AsNoTracking()
                .ProjectTo<ReadGoalDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalDto>>(goals);
        }

        public async Task<ApiResponse<IQueryable<ReadGoalDto>>> GetGoalsByWalletIdAndUserIdAsync(int walletId,string  userId)

        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);

            if (!walletExists)
                return new ApiResponse<IQueryable<ReadGoalDto>>("Wallet not found or doesn't belong to the user.");

            var goals = _unitOfWork.GoalRepository
                .GetByCondition(g => g.WalletId == walletId && g.Wallet.CreatorId == userId) 
                .AsNoTracking()
                .ProjectTo<ReadGoalDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadGoalDto>>(goals);
        }

        public async Task<ApiResponse<decimal>> GetTotalSavedAmountAsync(int goalId)
        {
            var goal = await _unitOfWork.GoalRepository.GetByIdAsync(goalId);

            if (goal == null)
                return new ApiResponse<decimal>("Goal not found.");

            return new ApiResponse<decimal>(goal.CurrentAmount);
        }


        public async Task<ApiResponse<decimal>> GetGoalProgressPercentageAsync(int goalId)
        {
            var goal = await _unitOfWork.GoalRepository.GetByIdAsync(goalId);
            if (goal == null)
                return new ApiResponse<decimal>("Goal not found.");

            if (goal.TargetAmount == 0)
                return new ApiResponse<decimal>("Target amount cannot be zero.");

            decimal percentage = (goal.CurrentAmount / goal.TargetAmount) * 100;

            percentage = Math.Min(percentage, 100);

            return new ApiResponse<decimal>(percentage);
        }
        public async Task<ApiResponse<decimal>> GetTotalGoalsProgressPercentageByWalletIdAsync(int walletId, string userId)
        {
           
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);
            if (!walletExists)
                return new ApiResponse<decimal>("Wallet not found or doesn't belong to the user.");

            var goals = await _unitOfWork.GoalRepository
                .GetByCondition(g => g.WalletId == walletId && g.Wallet.CreatorId == userId)
                .ToListAsync();

            if (!goals.Any())
                return new ApiResponse<decimal>("No goals found for the specified wallet.");

            decimal totalCurrentAmount = goals.Sum(g => g.CurrentAmount);
            decimal totalTargetAmount = goals.Sum(g => g.TargetAmount);

            if (totalTargetAmount == 0)
                return new ApiResponse<decimal>("Total target amount cannot be zero.");

            decimal overallPercentage = (totalCurrentAmount / totalTargetAmount) * 100;

          
            overallPercentage = Math.Min(overallPercentage, 100);

            return new ApiResponse<decimal>(overallPercentage);
        }

        public async Task<ApiResponse<(int totalGoals, decimal totalCurrentAmount)>> GetGoalsStatsByWalletIdAsync(int walletId, string userId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);
            if (!walletExists)
                return new ApiResponse<(int, decimal)>("Wallet not found or doesn't belong to the user.");

            var goals = _unitOfWork.GoalRepository.GetByCondition(g => g.WalletId == walletId && g.Wallet.CreatorId == userId);

            var totalGoals = await goals.CountAsync();
            var totalCurrentAmount = await goals.SumAsync(g => g.CurrentAmount);

            return new ApiResponse<(int, decimal)>((totalGoals, totalCurrentAmount));
        }









    }
}
