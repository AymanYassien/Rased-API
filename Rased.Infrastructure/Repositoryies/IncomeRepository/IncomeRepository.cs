using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public class IncomeRepository : Repository_Test<Income, int>, IIncomeRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<Income> _dbSet;
    
    public IncomeRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<Income>();
    }
    
    
    private IQueryable<Income> BuildBaseQuery(int walletId, bool isShared, Expression<Func<Income, bool>>[]? filter)
    {
        IQueryable<Income> query = _dbSet;
        
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

    public async Task<IQueryable<Income>> GetUserIncomesByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<Income, bool>>[]
            {
                x => x.SharedWalletId == walletId
            });
        
        return await GetAllAsync(new Expression<Func<Income, bool>>[]
        {
            x => x.WalletId == walletId
        });
    }

    public async Task<IQueryable<Income>> GetUserIncomesByWalletIdAsync(int walletId, Expression<Func<Income, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        IQueryable<Income> query =  _dbSet;
        
        if (filter != null)
            foreach (var fil in filter)
                query = query.Where(fil);

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

    public async Task<Income> GetUserIncomesAsync(int walletId, int incomeId, bool isShared = false)
    {
        if (isShared)
            return await GetAsync(new Expression<Func<Income, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                w => w.IncomeId == incomeId
            });
        
        
        return await GetAsync(new Expression<Func<Income, bool>>[]
        {
            x => x.WalletId == walletId,
            w => w.IncomeId == incomeId
        });
    }

    public async Task<decimal> CalculateTotalIncomesAmountAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        IQueryable<Income> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.SumAsync(e => e.Amount);

    }

    public async Task<decimal> CalculateTotalIncomesAmountForLastWeekAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        DateTime lastWeek = DateTime.UtcNow.AddDays(-7);
        IQueryable<Income> query = BuildBaseQuery(walletId, isShared, filter);
        
        query = query.Where(e => e.CreatedDate >= lastWeek);
        
        return await query.SumAsync(e => e.Amount);

    }

    public async Task<decimal> CalculateTotalIncomesAmountForLastMonthAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        DateTime lastMonth = DateTime.Now.AddMonths(-1); 
        IQueryable<Income> query = BuildBaseQuery(walletId, isShared, filter);

        
        query = query.Where(e => e.CreatedDate >= lastMonth);

        return await query.SumAsync(e => e.Amount);

    }

    public async Task<decimal> CalculateTotalIncomesAmountForLastYearAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null)
    {
        DateTime lastYear = DateTime.Now.AddYears(-1); 
        IQueryable<Income> query = BuildBaseQuery(walletId, isShared, filter);
        
        
        query = query.Where(e => e.CreatedDate >= lastYear);

        return await query.SumAsync(e => e.Amount);

    }

    public async Task<decimal> CalculateTotalIncomesAmountForSpecificPeriodAsync(int walletId, DateTime startDateTime, DateTime endDateTime,
        Expression<Func<Income, bool>>[]? filter = null, bool isShared = false)
    {
        if (startDateTime > endDateTime)
        {
            throw new ArgumentException("start Date  must be earlier than end Date.");
        }

        IQueryable<Income> query = BuildBaseQuery(walletId, isShared, filter);

        
        query = query.Where(e => e.CreatedDate >= startDateTime && e.CreatedDate <= endDateTime);

        return await query.SumAsync(e => e.Amount);

    }
}