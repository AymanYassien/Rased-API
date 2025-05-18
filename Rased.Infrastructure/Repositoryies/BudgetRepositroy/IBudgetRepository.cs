using System.Linq.Expressions;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;

public interface IBudgetRepository : IRepository_Test<Budget, int>
{
    Task<IQueryable<Budget>> GetBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<IQueryable<Budget>> GetValidBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<IQueryable<Budget>> GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(int walletId, DateTime startDate, DateTime endDate, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<int> CountValidBudgetsByWalletIdAsync(int walletId, bool isShared = false);
    Task<bool> IsBudgetValidAsync(int budgetId);
    Task<decimal> GetBudgetAmountAsync(int budgetId);
    Task<bool> IsBudgetRolloverAsync(int budgetId);
    Task<decimal> GetBudgetSpentAmountAsync(int budgetId);
    Task<bool> UpdateBudgetSpentAmountAsync(int budgetId, decimal newSpent);
    public Task<decimal> GetRemainingAmountAsync(int budgetId);

    Task<string> GetHighestBudgetExpensesAmountForWallet(int walletId, bool isShared);
    Task<string> GetLowestBudgetExpensesAmountForWallet(int walletId, bool isShared);
    Task<string> GetRemainderRatioForBudget(int walletId, bool isShared);
    Task<decimal> GetBudgetsAmountAndRatioAccordingWallet(int walletId, bool isShared = false);
    Task<decimal> GetTotalAmountsForWalletBudgets();

    public Task<(decimal totalIncome, decimal totalExpenses, int expensesOperationsNumber)> GetFinancialStatusAsync(
        int walletId, bool isShared = false);

    public Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphDataAsync(int walletId,
        bool isShared = false);

    public Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_YearlyAsync(
        int walletId, bool isShared = false);

    public Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_MonthlyAsync(
        int walletId, bool isShared = false);

    public Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_QuarterlyAsync(
        int walletId, bool isShared = false);


    Task<(decimal total, List<(string budget, decimal amount)>)> GetBudgetsStatisticsAsync(int walletId,
        bool isShared = false);
}