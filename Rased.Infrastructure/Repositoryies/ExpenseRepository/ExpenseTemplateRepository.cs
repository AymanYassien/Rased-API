using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public class ExpenseTemplateRepository : Repository_Test<ExpenseTemplate, int>, IExpenseTemplateRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<ExpenseTemplate> _dbSet;
    

    public ExpenseTemplateRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<ExpenseTemplate>();
    }
    

    public async Task<IQueryable<ExpenseTemplate>> GetUserExpensesTemplateByWalletIdAsync(int walletId, bool isShared = false)
    {
        if (isShared)
            return await GetAllAsync(new Expression<Func<ExpenseTemplate, bool>>[]
            {
                x => x.SharedWalletId == walletId
            });
        
        return await GetAllAsync(new Expression<Func<ExpenseTemplate, bool>>[]
        {
            x => x.WalletId == walletId
        });
    }

    
    public async Task<IQueryable<ExpenseTemplate>> GetUserExpensesTemplateByWalletIdAsync(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        IQueryable<ExpenseTemplate> query =  _dbSet;
        
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

    public async Task<ExpenseTemplate> GetUserExpenseAsync(int walletId, int expenseTemplateId, bool isShared = false)
    {
        if (isShared)
            return await GetAsync(new Expression<Func<ExpenseTemplate, bool>>[]
            {
                x => x.SharedWalletId == walletId,
                w => w.TemplateId == expenseTemplateId
            });
        
        
        return await GetAsync(new Expression<Func<ExpenseTemplate, bool>>[]
        {
            x => x.WalletId == walletId,
            w => w.TemplateId == expenseTemplateId
        });
    }

    public async Task<decimal> CalculateTotalExpensesTemplateAmountAsync(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null)
    {
        IQueryable<ExpenseTemplate> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.SumAsync(e => e.Amount);
    }

    public async Task<int> CountExpensesTemplateAsync(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        IQueryable<ExpenseTemplate> query = BuildBaseQuery(walletId, isShared, filter);

        return await query.CountAsync();
    }

    public async Task<int> CountExpensesTemplatesForAdminAsync(Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        IQueryable<ExpenseTemplate> query = _dbSet;
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
    
    private IQueryable<ExpenseTemplate> BuildBaseQuery(int walletId, bool isShared, Expression<Func<ExpenseTemplate, bool>>[]? filter)
    {
        IQueryable<ExpenseTemplate> query = _dbSet;
        
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