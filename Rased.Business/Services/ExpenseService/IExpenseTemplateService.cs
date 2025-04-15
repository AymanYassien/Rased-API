using System.Linq.Expressions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased.Business.Services.ExpenseService;

public interface IExpenseTemplateService
{
    // Basic CRUD
    public Task<ApiResponse<object>> GetUserExpenseTemplatesByWalletId(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetUserExpenseTemplate(int expenseTemplateId );
    public Task<ApiResponse<object>> AddUserExpenseTemplate(AddExpenseTemplateDto newExpenseTemplate);
    public Task<ApiResponse<object>> UpdateUserExpenseTemplate(int expenseTemplateId, UpdateExpenseTemplateDto updateExpenseTemplateDto );
    public Task<ApiResponse<object>> DeleteUserExpenseTemplate(int expenseTemplateId);
    
    // Count

    public Task<ApiResponse<object>> CountExpensesTemplate(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, 
        bool isShared = false);
    
    
    // Calculated  - maybe not heavy use those points - 
    public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmount(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
    // public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmountForLastWeek(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    // public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmountForLastMonth(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    // public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmountForLastYear(int walletId, bool isShared = false , Expression<Func<Expense, bool>>[]? filter = null);
    // public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmountForSpecificPeriod(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false);
    //
    
    // Filter Expenses 
    
    
    // For Admin 
    public Task<ApiResponse<object>> GetAllExpensesTemplatesForAdmin(Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
}