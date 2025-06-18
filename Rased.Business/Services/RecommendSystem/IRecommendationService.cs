using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.RecommendSystem
{
    public interface IRecommendationService
    {
        Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByUserAsync(string userId);
        Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByWalletAsync(int walletId);
        Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByWalletGroupAsync(int walletGroupId);
        Task<ApiResponse<string>> MarkRecommendationAsReadAsync(int recommendationId);
        Task<ApiResponse<string>> CreateRecommendationAsync(CreateBudgetRecommendationDto dto);
        Task GenerateMonthlyRecommendationsAsync();
    }

}
