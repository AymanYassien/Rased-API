using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased.Infrastructure.Repositoryies.Utility;

public class AutomationRuleRepository : Repository_Test<AutomationRule, int>, IAutomationRuleRepository
{
    private readonly RasedDbContext _context;
    private readonly DbSet<AutomationRule> _dbSet;
    

    public AutomationRuleRepository(RasedDbContext context) : base(context)
    {
        _context = context;
        _dbSet = _context.Set<AutomationRule>();
    }


    public async Task<IQueryable<AutomationRule>> GetUserAutomationRulesByWalletIdAsync(int walletId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0,
        int pageSize = 10, bool isShared = false)
    {
        IQueryable<AutomationRule> query =  _dbSet;
        
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

    public async Task<bool> IsAutomationRoleValid(int automationRuleId)
    {
        var automationRule = await GetByIdAsync(automationRuleId);
        if (automationRule is not null )
            return automationRule.EndDate > DateTime.UtcNow;
        return false;
    }

    public async Task DeactivateAutomationRole(int automationRuleId)
    {
        var automationRule = await GetByIdAsync(automationRuleId);
        if (automationRule is not null)
            automationRule.IsActive = false;
    }

    public async Task<int> CountAutomationRules(Expression<Func<AutomationRule, bool>>[]? filter = null)
    {
        IQueryable<AutomationRule> query = _dbSet;
        
        if (filter != null)
            foreach (var fil in filter)
                query = query.Where(fil);

        return await query.CountAsync();
    }
}