using System.Linq.Expressions;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public interface IExpenseTemplateRepository : IRepository_Test<ExpenseTemplate, int>
{
    
    Task<IQueryable<ExpenseTemplate>> GetUserExpensesTemplateByWalletIdAsync(int walletId, bool isShared = false);
    
    Task<IQueryable<ExpenseTemplate>> GetUserExpensesTemplateByWalletIdAsync(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<ExpenseTemplate> GetUserExpenseAsync(int walletId, int expenseTemplateId, bool isShared = false);
    
    Task<decimal> CalculateTotalExpensesTemplateAmountAsync(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
    
    // Complex + not need now !
    
    // Task<decimal> CalculateTotalExpensesTemplateAmountForLastWeekAsync(int walletId, bool isShared = false ,Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
    // Task<decimal> CalculateTotalExpensesTemplateAmountForLastMonthAsync(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
    // Task<decimal> CalculateTotalExpensesTemplateAmountForLastYearAsync(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null);
    // Task<decimal> CalculateTotalExpensesTemplateAmountForSpecificPeriodAsync(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, bool isShared = false);

    Task<int> CountExpensesTemplateAsync(int walletId,Expression<Func<ExpenseTemplate, bool>>[]? filter = null,  bool isShared = false);
    Task<int> CountExpensesTemplatesForAdminAsync(Expression<Func<ExpenseTemplate, bool>>[]? filter = null,  bool isShared = false);

}