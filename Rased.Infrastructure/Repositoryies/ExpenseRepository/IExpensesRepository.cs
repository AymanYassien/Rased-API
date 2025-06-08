using System.Linq.Expressions;
using Rased.Business;

using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public interface IExpensesRepository : IRepository_Test<Expense, int>
{
    Task<decimal> GetTotalExpensesAtMonthAsync(DateTime month);
    
    Task<IQueryable<Expense>> GetUserExpensesByWalletIdAsync(int walletId, bool isShared = false);
    
    Task<IQueryable<Expense>> GetUserExpensesByWalletIdAsync(int walletId, Expression<Func<Expense, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<Expense> GetUserExpenseAsync(int walletId, int expenseId, bool isShared = false);
    
    Task<decimal> CalculateTotalExpensesAmountAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null);
    Task<decimal> CalculateTotalExpensesAmountForLastWeekAsync(int walletId, bool isShared = false ,Expression<Func<Expense, bool>>[]? filter = null);
    Task<decimal> CalculateTotalExpensesAmountForLastMonthAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null);
    Task<decimal> CalculateTotalExpensesAmountForLastYearAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null);
    Task<decimal> CalculateTotalExpensesAmountForSpecificPeriodAsync(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false);
    // public Task<List<MonthlyExpenseSummary>> SumExpensesByMonthAsync(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false);
    Task<decimal> CalculateAverageDailyExpenseAmountForLastWeek(int walletId, bool isShared);
    // Task<decimal> CalculateAverageExpenseAmountForLastWeek(int walletId, bool isShared);
    Task<int> CountExpensesAsync(int walletId, bool isShared);
    Task<int> GetNumberOfRelatedBudgets(int walletId, bool isShared);
    Task<decimal> GetTotalAmountOfRelatedBudgets(int walletId, bool isShared);
    
    Task<IQueryable<Expense>> GetLast3ExpensesByBudgetId(int budgetId);
    Task<IQueryable<Expense>> GetLast10ExpensesByBudgetId(int budgetId);
    Task<IQueryable<Expense>> GetLast10ExpensesByWalletId(int walletId, bool isShared);
    
    // Task<List<MonthlyExpenseSummary>> SumExpensesByYearAsync(int walletId, bool isShared);
    
}