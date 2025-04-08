using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public class IncomeTemplateRepository : Repository_Test<IncomeTemplate, int>, IIncomeTemplateRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<IncomeTemplate> _dbSet;
    

    public IncomeTemplateRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<IncomeTemplate>();
    }


    public async Task<IQueryable<IncomeTemplate>> GetUserIncomesTemplateByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<IncomeTemplate, bool>>[]
            {
                x => x.SharedWalletId == walletId
            });
        
        return await GetAllAsync(new Expression<Func<IncomeTemplate, bool>>[]
        {
            x => x.WalletId == walletId
        });
    }

    public async Task<IQueryable<IncomeTemplate>> GetUserIncomesTemplateByWalletIdAsync(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, int pageNumber = 0,
        int pageSize = 10, bool isShared = false)
    {
        IQueryable<IncomeTemplate> query =  _dbSet;
        
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

    public async Task<IncomeTemplate> GetUserIncomesAsync(int walletId, int incomeTemplateId, bool isShared = false)
    {
        if (isShared)
            return await GetAsync(new Expression<Func<IncomeTemplate, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                w => w.IncomeTemplateId == incomeTemplateId
            });
        
        
        return await GetAsync(new Expression<Func<IncomeTemplate, bool>>[]
        {
            x => x.WalletId == walletId,
            w => w.IncomeTemplateId == incomeTemplateId
        });
    }

    public async Task<decimal> CalculateTotalIncomesTemplateAmountAsync(int walletId, bool isShared = false, Expression<Func<IncomeTemplate, bool>>[]? filter = null)
    {
        IQueryable<IncomeTemplate> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.SumAsync(e => e.Amount);
    }

    public async Task<int> CountIncomesTemplateAsync(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, bool isShared = false)
    {
        IQueryable<IncomeTemplate> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.CountAsync();
    }

    public async Task<int> CountIncomesTemplatesForAdminAsync(Expression<Func<IncomeTemplate, bool>>[]? filter = null, bool isShared = false)
    {
        IQueryable<IncomeTemplate> query = _dbSet;
        if (isShared)
        {
            query = query.Where(e => e.SharedWalletId != null);
        }
        
        if (filter != null && filter.Length > 0)
        {
            foreach (var expression in filter)
            {
                query = query.Where(expression);
            }
        }
        

        return await query.CountAsync();
    }
    
    private IQueryable<IncomeTemplate> BuildBaseQuery(int walletId, bool isShared, Expression<Func<IncomeTemplate, bool>>[]? filter)
    {
        IQueryable<IncomeTemplate> query = _dbSet;
        
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