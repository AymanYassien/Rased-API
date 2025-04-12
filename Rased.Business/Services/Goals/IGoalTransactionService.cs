using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Goals
{
    public interface IGoalTransactionService
    {
        Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetAllGoalsTransactionAsync();
        Task<ApiResponse<ReadGoalTransactionDto?>> GetGoalTransactionByIdAsync(int id);
        Task<ApiResponse<string>> AddGoalTransactionAsync(AddGoalTransactionDto addGoalTransactionDto);
        Task<ApiResponse<string>> UpdateGoalTransactionAsync( UpdateGoalTransactionDto updateGoalTransactionDto);
        Task<ApiResponse<string>> DeleteGoalTransactionAsync(int id);

    
        Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetTransactionsByGoalIdAsync(int goalId);
        Task<ApiResponse<IQueryable<ReadGoalTransactionDto>>> GetTransactionsByWalletIdAsync(int walletId);
        Task<ApiResponse<bool>> IsGoalCompletedAsync(int goalId);
        Task<ApiResponse<decimal>> GetTotalSavedAmountByDateRangeAsync(int goalId, DateTime startDate, DateTime endDate);



    }
}
