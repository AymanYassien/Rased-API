using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Business;
using Rased.Infrastructure;
using Rased.Infrastructure.Models;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public class ExpenseRepository : Repository_Test<Expense, int>, IExpensesRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<Expense> _dbSet;
    

    public ExpenseRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Expense>();
    }


    // need Modify - not for use, just Testing !
    public async Task<decimal> GetTotalExpensesAtMonthAsync(DateTime month)
    {
        var startOfMonth = new DateTime(month.Year, month.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // can handle date > 28
        return await _context.Expenses
            .Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth)
            .SumAsync(e => e.Amount);
    }

    public async Task<IQueryable<Expense>> GetUserExpensesByWalletIdAsync(int walletId, bool isShared = false)
    {

        if (isShared)
            return await GetAllAsync(new Expression<Func<Expense, bool>>[]
            {
                x => x.SharedWalletId == walletId
            });
        
        return await GetAllAsync(new Expression<Func<Expense, bool>>[]
        {
            x => x.WalletId == walletId
        });

    }
    public async Task<IQueryable<Expense>> GetUserExpensesByWalletIdAsync(int walletId, Expression<Func<Expense, bool>>[]? filter = null,
        int pageNumber = 0, int pageSize = 10,  bool isShared = false)
    {
        IQueryable<Expense> query =  _dbSet;
        
        if (filter != null)
            foreach (var fil in filter)
                query = query.Include(x => x.StaticPaymentMethodsData).Where(fil);

        if (isShared)
            query = query.Where(x => x.SharedWalletId == walletId);
        else
            query = query.Where(x => x.WalletId == walletId);
        
        if (pageSize > 0)
        {
            if (pageSize > 100) pageSize = 100; // Cap page size
            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        var items =  query;

        return query;
        
    }

    public async Task<Expense> GetUserExpenseAsync(int walletId, int expenseId, bool isShared = false)
    {
        if (isShared)
            return await GetAsync(new Expression<Func<Expense, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                w => w.ExpenseId == expenseId
            });
        
        
        return await GetAsync(new Expression<Func<Expense, bool>>[]
        {
            x => x.WalletId == walletId,
            w => w.ExpenseId == expenseId
        });
        
    }

    public async Task<decimal> CalculateTotalExpensesAmountAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {

        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.SumAsync(e => e.Amount);

    }

    public async Task<decimal> CalculateTotalExpensesAmountForLastWeekAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);
        
        query = query.Where(e => e.Date >= lastWeek);
        
        return await query.SumAsync(e => e.Amount);
    }

    public async Task<decimal> CalculateTotalExpensesAmountForLastMonthAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        DateTime lastMonth = DateTime.Now.AddMonths(-1); 
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);

        
        query = query.Where(e => e.Date >= lastMonth);

        return await query.SumAsync(e => e.Amount);
    }

    public async Task<decimal> CalculateTotalExpensesAmountForLastYearAsync(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        DateTime lastYear = DateTime.Now.AddYears(-1); 
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);
        
        
        query = query.Where(e => e.Date >= lastYear);

        return await query.SumAsync(e => e.Amount);
    }

    public async Task<decimal> CalculateTotalExpensesAmountForSpecificPeriodAsync(int walletId, DateTime startDateTime, DateTime endDateTime,
        Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false)
    {
        if (startDateTime > endDateTime)
        {
            throw new ArgumentException("startDateTime must be earlier than endDateTime.");
        }

        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);

        
        query = query.Where(e => e.Date >= startDateTime && e.Date <= endDateTime);

        return await query.SumAsync(e => e.Amount);
    }

    // public async Task<List<MonthlyExpenseSummary>> SumExpensesByMonthAsync(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Expense, bool>>[]? filter = null, bool isShared = false)
    // {
    //     if (startDateTime > endDateTime)
    //     {
    //         throw new ArgumentException("startDateTime must be earlier than endDateTime.");
    //     }
    //
    //     IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, filter);
    //
    //     // Filter expenses within the specified date range
    //     query = query.Where(e => e.Date >= startDateTime && e.Date <= endDateTime);
    //
    //     
    //     var result = await query
    //         .GroupBy(e => new { e.Date.Year, e.Date.Month })
    //         .Select(g => new MonthlyExpenseSummary
    //         {
    //             Year = g.Key.Year,
    //             Month = g.Key.Month,
    //             TotalAmount = g.Sum(e => e.Amount)
    //         })
    //         .OrderBy(g => g.Year).ThenBy(g => g.Month)
    //         .ToListAsync();
    //     
    //     return result;
    //     
    // }

    public async Task<decimal> CalculateAverageDailyExpenseAmountForLastWeek(int walletId, bool isShared)
    {
        DateTime endDate = DateTime.UtcNow;
        DateTime startDate = endDate.AddDays(-7);
        
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, null);
        
        query = query.Where(e => e.Date >= startDate && e.Date <= endDate);
        
        decimal totalAmount = await query.SumAsync(e => e.Amount);
        
        if (totalAmount == 0) return totalAmount;
        
        decimal averageDailyExpense = totalAmount / 7;

        return averageDailyExpense;
    }

    public async Task<int> CountExpensesAsync(int walletId, bool isShared)
    {
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, null);

        var total = await query.CountAsync();

        return total;

    }

    public async Task<int> GetNumberOfRelatedBudgets(int walletId, bool isShared)
    {
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, null);
        var total = await query.GroupBy(g => g.RelatedBudgetId).CountAsync();

        return total;
    }

    public async Task<decimal> GetTotalAmountOfRelatedBudgets(int walletId, bool isShared)
    {
        IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, null);
        var Sum = await query.SumAsync(e => e.Amount);

        return Sum;
    }

    public async Task<IQueryable<Expense>> GetLast3ExpensesByBudgetId(int budgetId)
    {
        IQueryable<Expense> expenses =  _dbSet
            .Where(x => x.RelatedBudgetId == budgetId)
            .OrderByDescending(x => x.Date)
            .Take(3);
         return expenses;
        //return Task.FromResult(expenses);    => with no async
    }
    

    public async Task<IQueryable<Expense>> GetLast10ExpensesByWalletId(int walletId, bool isShared)
    {
        IQueryable<Expense> expenses;
        if (isShared)
            expenses =  _dbSet
                .Include(x => x.SubCategory)
                .Include(x => x.StaticPaymentMethodsData)
                .Include(x => x.RelatedBudget)
                .Where(x => x.SharedWalletId == walletId)
                .OrderByDescending(x => x.Date)
                .Take(10);
        
            
        else
            expenses =  _dbSet
            .Include(x => x.SubCategory)
            .Include(x => x.StaticPaymentMethodsData)
            .Include(x => x.RelatedBudget)
            .Where(x => x.WalletId == walletId)
            .OrderByDescending(x => x.Date)
            .Take(10);
        
        return expenses;
    }
    
    public async Task<IQueryable<Expense>> GetLast10ExpensesByBudgetId(int budgetId)
    {
        IQueryable<Expense> expenses =  _dbSet
            .Where(x => x.RelatedBudgetId == budgetId)
            .OrderByDescending(x => x.Date)
            .Take(10);
        return expenses;
    }


    // public async Task<List<MonthlyExpenseSummary>> SumExpensesByYearAsync(int walletId, bool isShared)
    // {
    //     
    //     IQueryable<Expense> query = BuildBaseQuery(walletId, isShared, null);
    //     
    //     
    //     var result = await query
    //         .GroupBy(e => new { e.Date.Year })
    //         .Select(g => new MonthlyExpenseSummary
    //         {
    //             Year = g.Key.Year,
    //             TotalAmount = g.Sum(e => e.Amount)
    //         })
    //         .OrderBy(g => g.Year)
    //         .ToListAsync();
    //
    //     return result;
    // }

    private IQueryable<Expense> BuildBaseQuery(int walletId, bool isShared, Expression<Func<Expense, bool>>[]? filter)
    {
        IQueryable<Expense> query = _dbSet;
        
        if (isShared)
        {
            query = query.Where(e => e.SharedWalletId == walletId);
        }
        else
        {
            query = query.Where(e => e.WalletId == walletId);
        }
        
        
        if (filter != null && filter.Length > 0)
        {
            foreach (var expression in filter)
            {
                query = query.Where(expression);
            }
        }

        return query;
    }

    
}