using System.Linq.Expressions;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased.Infrastructure.Repositoryies.Utility;

public interface IAutomationRuleRepository : IRepository_Test<AutomationRule, int>
{

    Task<IQueryable<AutomationRule>> GetUserAutomationRulesByWalletIdAsync(int walletId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<bool> IsAutomationRoleValid(int automationRuleId);
    Task DeactivateAutomationRole(int automationRuleId);
    Task<int> CountAutomationRules(Expression<Func<AutomationRule, bool>>[]? filter = null);

    
}