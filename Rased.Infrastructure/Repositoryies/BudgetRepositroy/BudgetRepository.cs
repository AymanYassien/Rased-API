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
                x => x.EndDate < DateTime.UtcNow
            });
        
        return await GetAllAsync(new Expression<Func<Budget, bool>>[]
        {
            x => x.WalletId == walletId,
            x => x.EndDate < DateTime.UtcNow
        });
    }

    public async Task<IQueryable<Budget>> GetBudgetsByWalletIdCategorizedAtSpecificPeriodAsync(int walletId, DateTime startDate, DateTime endDate,
        Expression<Func<Budget, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10, bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<Budget, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                x => x.EndDate ==  endDate,
                x => x.StartDate ==  startDate,
            });
        
        return await GetAllAsync(new Expression<Func<Budget, bool>>[]
        {
            x => x.WalletId == walletId,
            x => x.EndDate ==  endDate,
            x => x.StartDate ==  startDate,
        });
    }

    public async Task<int> CountValidBudgetsByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (isShared)
            return await _dbSet.Where(x => x.SharedWalletId == walletId).CountAsync();
        
        return await _dbSet.Where(x => x.WalletId == walletId).CountAsync();
        
    }

    public async Task<bool> IsBudgetValidAsync(int budgetId)
    {
        var res =  _dbSet.Where(x => x.BudgetId == budgetId && x.EndDate < DateTime.UtcNow);

        return res != null;
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
}