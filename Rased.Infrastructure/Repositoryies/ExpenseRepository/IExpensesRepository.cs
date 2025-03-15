using System.Linq.Expressions;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
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

}