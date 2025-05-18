using System.Linq.Expressions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased.Business.Services.ExpenseService;

public interface IExpenseService
{
    // Basic CRUD
    public Task<ApiResponse<object>> GetUserExpensesByWalletId(int walletId, Expression<Func<Expense, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetUserExpense(int expenseId );
    public Task<ApiResponse<object>> AddUserExpense(AddExpenseDto newExpenseDto);
    public Task<ApiResponse<object>> UpdateUserExpense(int expenseId,UpdateExpenseDto updateExpenseDto);
    public Task<ApiResponse<object>> DeleteUserExpense(int expenseId);
    
    // Calculated 
    public Task<ApiResponse<object>> CalculateTotalExpensesAmount(int walletId, bool isShared = false ,Expression<Func<Expense, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastWeek(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastMonth(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalExpensesAmountForLastYear(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    public Task<ApiResponse<object>> CalculateTotalExpensesAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false);

    public Task<int> AddUserExpense_forInternalUsage(AddExpenseDto newExpenseDto);
    
    // Filter Expenses 
    
    
    // For Admin 
    public Task<ApiResponse<object>> GetAllExpensesForAdmin(
        Expression<Func<Expense, bool>>[]? filter = null,
        Expression<Func<Expense, object>>[]? includes = null,
        int pageNumber = 0,
        int pageSize = 10);
    
}

