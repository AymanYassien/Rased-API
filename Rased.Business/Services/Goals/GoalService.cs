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
            await _unitOfWork.GoalRepository.AddAsync(_mapper.Map<Goal>(addGoalDto));
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
            var goalExists = await _unitOfWork.GoalRepository.AnyAsync(g => g.Id == goalId);
            if (!goalExists)
                return new ApiResponse<decimal>("Goal not found.");


            var totalSavedAmount = await _unitOfWork.GoalTransactionRepository
                .GetByCondition(t => t.GoalId == goalId)
                .SumAsync(t => t.InsertedAmount);  

            return new ApiResponse<decimal>(totalSavedAmount);
        }

     




    }
}
