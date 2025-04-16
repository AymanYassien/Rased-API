using System.Linq.Expressions;
using Rased_API.Rased.Infrastructure;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public interface IIncomeService
{
    // Basic CRUD
    public Task<ApiResponse<object>> GetUserIncomesByWalletId(int walletId, Expression<Func<Income, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetUserIncome(int incomeId);
    public Task<ApiResponse<object>> AddUserIncome(AddIncomeDto newIncomeDto);
    public Task<ApiResponse<object>> UpdateUserIncome(int incomeId,UpdateIncomeDto updateIncomeDto);
    public Task<ApiResponse<object>> DeleteUserIncome(int incomeId);
    
    // Calculated 
    public Task<ApiResponse<object>> CalculateTotalIncomesAmount(int walletId, bool isShared = false ,Expression<Func<Income, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastWeek(int walletId, bool isShared = false , Expression<Func<Income, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastMonth(int walletId, bool isShared = false , Expression<Func<Income, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalIncomesAmountForLastYear(int walletId, bool isShared = false , Expression<Func<Income, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalIncomesAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Income, bool>>[]? filter = null, bool isShared = false);
    
    
    // Filter Incomes 
    
    
    // For Admin 
    public Task<ApiResponse<object>> GetAllIncomesForAdmin(
        Expression<Func<Income, bool>>[]? filter = null,
        Expression<Func<Income, object>>[]? includes = null,
        int pageNumber = 0,
        int pageSize = 10);
}