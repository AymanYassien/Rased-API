using System.Linq.Expressions;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased_API.Rased.Business.Services.BudgetService;


public interface IBudgetService
{
    Task<ApiResponse<object>> GetBudgetsById(int budgetId);
    
    Task<ApiResponse<object>> AddBudgetAsync(AddBudgetDto dto);
    Task<ApiResponse<object>> UpdateBudgetAsync(int budgetId, UpdateBudgetDto dto);
    Task<ApiResponse<object>> DeleteBudgetAsync(int id);
    
    Task<ApiResponse<object>> GetBudgetsForAdminAsync(Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10);
    
    Task<ApiResponse<object>> GetBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<ApiResponse<object>> GetValidBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<ApiResponse<object>> GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(int walletId, DateTime startDate, DateTime endDate, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<ApiResponse<object>> CountValidBudgetsByWalletIdAsync(int walletId, bool isShared = false);
    Task<ApiResponse<object>> IsBudgetValidAsync(int budgetId);
    Task<ApiResponse<object>> GetBudgetAmountAsync(int budgetId);
    Task<ApiResponse<object>> IsBudgetRolloverAsync(int budgetId);
    Task<ApiResponse<object>> GetBudgetSpentAmountAsync(int budgetId);
    Task<ApiResponse<object>> UpdateBudgetSpentAmountAsync(int budgetId, decimal newSpent);
    public Task<ApiResponse<object>> GetRemainingAmountAsync(int budgetId);

    public Task<ApiResponse<object>> GetFinancialStatusAsync(int walletId, bool isShared = false);
    Task<ApiResponse<object>> GetFinancialGraphDataAsync(int walletId, bool isShared = false);

    Task<ApiResponse<object>> GetBudgetsStatisticsAsync(int walletId, bool isShared = false);
    public Task<string> GetBudgetNameById(int id);
}
