using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;

public class BudgetRepository : Repository_Test<Budget, int>, IBudgetRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<Budget> _dbSet;

    public BudgetRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Budget>();
    }


    public async Task<IQueryable<Budget>> GetBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<Budget, bool>>[]
            {
                x => x.SharedWalletId == walletId
            });
        
        return await GetAllAsync(new Expression<Func<Budget, bool>>[]
        {
            x => x.WalletId == walletId
        });
    }

    public async Task<IQueryable<Budget>> GetValidBudgetsByWalletIdAsync(int walletId, Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<Budget, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                x => x.EndDate > DateTime.UtcNow
            });
        
        return await GetAllAsync(new Expression<Func<Budget, bool>>[]
        {
            x => x.WalletId == walletId,
            x => x.EndDate > DateTime.UtcNow
        });
    }

    public async Task<IQueryable<Budget>> GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(int walletId, DateTime startDate, DateTime endDate,
        Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10, bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<Budget, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                x => x.EndDate <  endDate,
                x => x.StartDate >  startDate
            });
        
        return await GetAllAsync(new Expression<Func<Budget, bool>>[]
        {
            x => x.WalletId == walletId,
            x => x.EndDate <  endDate,
            x => x.StartDate >  startDate
        });
    }

    public async Task<int> CountValidBudgetsByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (isShared)
            return await _dbSet.Where(x => x.SharedWalletId == walletId).Select(x => x.EndDate > DateTime.UtcNow).CountAsync();
        
        return await _dbSet.Where(x => x.SharedWalletId == walletId).Select(x => x.EndDate > DateTime.UtcNow).CountAsync();
    }

    public async Task<bool> IsBudgetValidAsync(int budgetId)
    {
        var res =  _dbSet.Where(x => x.BudgetId == budgetId && x.EndDate > DateTime.UtcNow);

        return res.Any();
    }

    public async Task<decimal> GetBudgetAmountAsync(int budgetId)
    {
        IQueryable<Budget> query = _dbSet;
        query = query.Where(e => e.BudgetId == budgetId);
        return await query.SumAsync(e => e.BudgetAmount);
    }
    
    public async Task<decimal> GetRemainingAmountAsync(int budgetId)
    {
        IQueryable<Budget> query = _dbSet;
        query = query.Where(e => e.BudgetId == budgetId);
        return await query.SumAsync(e => e.RemainingAmount);
    }

    public Task<string> GetHighestBudgetExpensesAmountForWallet(int walletId, bool isShared)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetLowestBudgetExpensesAmountForWallet(int walletId, bool isShared)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetRemainderRatioForBudget(int walletId, bool isShared)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetBudgetsAmountAndRatioAccordingWallet(int walletId, bool isShared = false)
    {
        throw new NotImplementedException();
    }

    public Task<decimal> GetTotalAmountsForWalletBudgets()
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsBudgetRolloverAsync(int budgetId)
    {
        IQueryable<Budget> query = _dbSet;
        query = query.Where(e => e.BudgetId == budgetId);
        return  query.First().RolloverUnspent;
    }

    public async Task<decimal> GetBudgetSpentAmountAsync(int budgetId)
    {
        IQueryable<Budget> query = _dbSet;
        query = query.Where(e => e.BudgetId == budgetId);
        return await query.SumAsync(e => e.SpentAmount);
    }

    public async Task<bool> UpdateBudgetSpentAmountAsync(int budgetId, decimal newSpent )
    {
        var obj = await GetByIdAsync(budgetId);
        
        // -50 : 90
        if (obj is not null)
        {
            obj.SpentAmount += newSpent;
            obj.RemainingAmount -= newSpent;

            if (obj.RemainingAmount < 0)
                obj.RolloverUnspent = false;
            
            Update(obj);
            _context.SaveChanges();
            return true;
        }

        return false;
    }
    
    public async Task<(decimal totalIncome, decimal totalExpenses, int expensesOperationsNumber)> GetFinancialStatusAsync(int walletId, bool isShared = false)
    {
        var incomeQuery = isShared
            ? _context.Incomes
                .Where(i => i.SharedWalletId == walletId)
                .SumAsync(i => (decimal?)i.Amount) ?? Task.FromResult<decimal?>(0m)
            : _context.Incomes
                .Where(i => i.WalletId == walletId)
                .SumAsync(i => (decimal?)i.Amount) ?? Task.FromResult<decimal?>(0m);

        var expensesQuery = isShared
            ? _context.Expenses
                .Where(e => e.SharedWalletId == walletId)
            : _context.Expenses
                .Where(e => e.WalletId == walletId);

        var totalExpensesQuery = expensesQuery
            .SumAsync(e => (decimal?)e.Amount) ?? Task.FromResult<decimal?>(0m);

        var expensesCountQuery = expensesQuery.CountAsync();

        // Execute queries concurrently
        await Task.WhenAll(incomeQuery, totalExpensesQuery, expensesCountQuery);

        // Extract results
        var totalIncome = await incomeQuery ?? 0m;
        var totalExpenses = await totalExpensesQuery ?? 0m;
        var expensesOperationsNumber = await expensesCountQuery;

        return (totalIncome, totalExpenses, expensesOperationsNumber);

    }
    
    
     public async Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphDataAsync(int walletId, bool isShared = false)
        {
            var currentDate = DateTime.UtcNow;
            var threeYearsAgo = DateTime.UtcNow.AddYears(-3).Year;
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
            var fourQuartersAgo = currentDate.AddMonths(-12);
            // Base queries
            var incomeQuery = isShared
                ? _context.Incomes.Where(i => i.SharedWalletId == walletId)
                : _context.Incomes.Where(i => i.WalletId == walletId);

            var expenseQuery = isShared
                ? _context.Expenses.Where(e => e.SharedWalletId == walletId)
                : _context.Expenses.Where(e => e.WalletId == walletId);

            // Yearly 
            var yearlyIncome = await incomeQuery
                .Where(i => i.CreatedDate.Year >= threeYearsAgo)
                .GroupBy(i => i.CreatedDate.Year)
                .Select(g => new { Period = g.Key.ToString(), Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var yearlyExpense = await expenseQuery
                .Where(e => e.Date.Year >= threeYearsAgo)
                .GroupBy(e => e.Date.Year)
                .Select(g => new { Period = g.Key.ToString(), Expense = g.Sum(e => e.Amount) })
                .ToListAsync();

            // Quarterly data (all quarters in last 3 years)
            var quarterlyIncomeTask = await incomeQuery
                .Where(i => i.CreatedDate >= fourQuartersAgo)
                .GroupBy(i => new { i.CreatedDate.Year, Quarter = (i.CreatedDate.Month - 1) / 3 + 1 })
                .Select(g => new { Period = $"{g.Key.Year}-Q{g.Key.Quarter}", Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var quarterlyExpenseTask = await expenseQuery
                .Where(e => e.Date >= fourQuartersAgo)
                .GroupBy(e => new { e.Date.Year, Quarter = (e.Date.Month - 1) / 3 + 1 })
                .Select(g => new { Period = $"{g.Key.Year}-Q{g.Key.Quarter}", Expense = g.Sum(e => e.Amount) })
                .ToListAsync();

            // Monthly data (last 6 months)
            var monthlyIncome = await incomeQuery
                .Where(i => i.CreatedDate >= sixMonthsAgo)
                .GroupBy(i => new { i.CreatedDate.Year, i.CreatedDate.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var monthlyExpense = await expenseQuery
                .Where(e => e.Date >= sixMonthsAgo)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Expense = g.Sum(e => e.Amount) })
                .ToListAsync();

            // Combine results


            var monthlyPeriods = monthlyIncome.Select(i => i.Period)
                .Union(monthlyExpense.Select(e => e.Period))
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            var quarterlyPeriods = quarterlyIncomeTask.Select(i => i.Period)
                .Union(quarterlyExpenseTask.Select(e => e.Period))
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            var yearlyPeriods = yearlyIncome.Select(i => i.Period)
                .Union(yearlyExpense.Select(e => e.Period))
                .Distinct()
                .OrderBy(p => p)
                .ToList();

            // Combine periods in desired order: months, quarters, years
            var orderedPeriods = monthlyPeriods
                .Concat(quarterlyPeriods)
                .Concat(yearlyPeriods)
                .Distinct()
                .ToList();

            var result = orderedPeriods.Select(p => (
                period: p,
                income: monthlyIncome.FirstOrDefault(i => i.Period == p)?.Income
                        ?? quarterlyIncomeTask.FirstOrDefault(i => i.Period == p)?.Income
                        ?? yearlyIncome.FirstOrDefault(i => i.Period == p)?.Income
                        ?? 0m,
                expense: monthlyExpense.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? quarterlyExpenseTask.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? yearlyExpense.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? 0m
            )).ToList();

            return result;
        }

        public async Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_YearlyAsync(
            int walletId, bool isShared = false)
        {
            var threeYearsAgo = DateTime.UtcNow.AddYears(-3).Year;
            // Base queries
            var incomeQuery = isShared
                ? _context.Incomes.Where(i => i.SharedWalletId == walletId)
                : _context.Incomes.Where(i => i.WalletId == walletId);

            var expenseQuery = isShared
                ? _context.Expenses.Where(e => e.SharedWalletId == walletId)
                : _context.Expenses.Where(e => e.WalletId == walletId);

            // Yearly 
            var yearlyIncome = await incomeQuery
                .Where(i => i.CreatedDate.Year >= threeYearsAgo)
                .GroupBy(i => i.CreatedDate.Year)
                .Select(g => new { Period = g.Key.ToString(), Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var yearlyExpense = await expenseQuery
                .Where(e => e.Date.Year >= threeYearsAgo)
                .GroupBy(e => e.Date.Year)
                .Select(g => new { Period = g.Key.ToString(), Expense = g.Sum(e => e.Amount) })
                .ToListAsync();
            
            var periods = yearlyIncome.Select(i => i.Period)
                .Union(yearlyExpense.Select(e => e.Period))
                .Distinct()
                .ToList();

            var result = periods.Select(p => (
                period: p,
                income: yearlyIncome.FirstOrDefault(i => i.Period == p)?.Income
                        ?? 0m,
                expense: yearlyExpense.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? 0m
            )).OrderBy(r => r.period).ToList();

            return result;
        }
        
        public async Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_MonthlyAsync(
            int walletId, bool isShared = false)
        {
            var currentDate = DateTime.UtcNow;
            var sixMonthsAgo = DateTime.UtcNow.AddMonths(-6);
            // Base queries
            var incomeQuery = isShared
                ? _context.Incomes.Where(i => i.SharedWalletId == walletId)
                : _context.Incomes.Where(i => i.WalletId == walletId);

            var expenseQuery = isShared
                ? _context.Expenses.Where(e => e.SharedWalletId == walletId)
                : _context.Expenses.Where(e => e.WalletId == walletId);

            
            // Monthly data (last 6 months)
            var monthlyIncome = await incomeQuery
                .Where(i => i.CreatedDate >= sixMonthsAgo)
                .GroupBy(i => new { i.CreatedDate.Year, i.CreatedDate.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var monthlyExpense = await expenseQuery
                .Where(e => e.Date >= sixMonthsAgo)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new { Period = $"{g.Key.Year}-{g.Key.Month:D2}", Expense = g.Sum(e => e.Amount) })
                .ToListAsync();

            // Combine results


            var periods = monthlyIncome.Select(i => i.Period)
                .Union(monthlyExpense.Select(e => e.Period))
                .Distinct()
                .ToList();

            var result = periods.Select(p => (
                period: p,
                income:  monthlyIncome.FirstOrDefault(i => i.Period == p)?.Income
                        ?? 0m,
                expense:  monthlyExpense.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? 0m
            )).OrderBy(r => r.period).ToList();

            return result;
        }
        
        public async Task<List<(string period, decimal income, decimal expense)>> GetFinancialGraphData_QuarterlyAsync(
            int walletId, bool isShared = false)
        {
            var currentDate = DateTime.UtcNow;
            var fourQuartersAgo = currentDate.AddMonths(-12);
            // Base queries
            var incomeQuery = isShared
                ? _context.Incomes.Where(i => i.SharedWalletId == walletId)
                : _context.Incomes.Where(i => i.WalletId == walletId);

            var expenseQuery = isShared
                ? _context.Expenses.Where(e => e.SharedWalletId == walletId)
                : _context.Expenses.Where(e => e.WalletId == walletId);

            
            // Quarterly data (all quarters in last 3 years)
            var quarterlyIncomeTask = await incomeQuery
                .Where(i => i.CreatedDate >= fourQuartersAgo)
                .GroupBy(i => new { i.CreatedDate.Year, Quarter = (i.CreatedDate.Month - 1) / 3 + 1 })
                .Select(g => new { Period = $"{g.Key.Year}-Q{g.Key.Quarter}", Income = g.Sum(i => i.Amount) })
                .ToListAsync();

            var quarterlyExpenseTask = await expenseQuery
                .Where(e => e.Date >= fourQuartersAgo)
                .GroupBy(e => new { e.Date.Year, Quarter = (e.Date.Month - 1) / 3 + 1 })
                .Select(g => new { Period = $"{g.Key.Year}-Q{g.Key.Quarter}", Expense = g.Sum(e => e.Amount) })
                .ToListAsync();

            // Combine results


            var periods = quarterlyIncomeTask.Select(i => i.Period)
                .Union(quarterlyExpenseTask.Select(e => e.Period))
                .Distinct()
                .ToList();

            var result = periods.Select(p => (
                period: p,
                income:  quarterlyIncomeTask.FirstOrDefault(i => i.Period == p)?.Income
                        
                        ?? 0m,
                expense: quarterlyExpenseTask.FirstOrDefault(e => e.Period == p)?.Expense
                         ?? 0m
            )).OrderBy(r => r.period).ToList();

            return result;
        }
        
        public async Task<(decimal total, List<(string budget, decimal amount)>)> GetBudgetsStatisticsAsync(int walletId, bool isShared = false)
        {
            
            var budgetQuery = isShared
                ? _context.Budgets.Where(e => e.SharedWalletId == walletId)
                : _context.Budgets.Where(e => e.WalletId == walletId);

            
            var totalTask = budgetQuery
                .SumAsync(e => (decimal?)e.BudgetAmount) ?? Task.FromResult<decimal?>(0m);

           
            var groupedTask = budgetQuery
                .Select(g => new { Budget = g.Name, Amount = g.BudgetAmount })
                .OrderBy(g => g.Budget)
                .ToListAsync();

            // Execute queries concurrently
            await Task.WhenAll(totalTask, groupedTask);

            // Extract results
            var total = await totalTask ?? 0m;
            var grouped = await groupedTask;

            // Map grouped results to tuple list
            var budgetExpenses = grouped
                .Select(g => (budget: g.Budget ?? "Uncategorized", amount: g.Amount)) 
                .ToList();

            return (total, budgetExpenses);
        }
        
        
}