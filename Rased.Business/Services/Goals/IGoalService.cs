using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure.Models.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Goals
{
    public interface IGoalService
    {
        Task<ApiResponse<IQueryable<ReadGoalDto>>> GetAllGoalsAsync();
        Task<ApiResponse<ReadGoalDto?>> GetGoalByIdAsync(int id);
        Task<ApiResponse<string>> AddGoalAsync(AddGoalDto addGoalDto);
        Task<ApiResponse<string>> UpdateGoalAsync(UpdateGoalDto updateGoalDto);
        Task<ApiResponse<string>> DeleteGoalAsync(int id);

        Task<ApiResponse<IQueryable<ReadGoalDto>>> GetGoalsByStatusAsync(GoalStatusEnum status);
        Task<ApiResponse<IQueryable<ReadGoalDto>>> GetGoalsByWalletIdAndUserIdAsync(int walletId, string userId);
        Task<ApiResponse<decimal>> GetTotalSavedAmountAsync(int goalId);



    }
}