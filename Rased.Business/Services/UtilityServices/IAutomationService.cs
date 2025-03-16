using System.Linq.Expressions;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace Rased_API.Rased.Business;

public interface IAutomationService
{
    public Task<ApiResponse<object>> GetUserAutomationRulesByWalletIdAsync(int walletId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetAutomationRuleById(int automationId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10);
    public Task<ApiResponse<object>> AddAutomationRule(AutomationRule newAutomationRule);
    public Task<ApiResponse<object>> UpdateUserAutomationRule(int ruleId,AutomationRule updateAutomationRule);
    public Task<ApiResponse<object>> DeleteAutomationRule(int ruleId);
    
    
    Task<ApiResponse<object>> IsAutomationRoleValid(int automationRuleId);
    Task<ApiResponse<object>> DeactivateAutomationRole(int automationRuleId);
    Task<ApiResponse<object>> CountAutomationRules(Expression<Func<AutomationRule, bool>>[]? filter = null);
    
    
    // For Admin 
    public Task<ApiResponse<object>> GetAllAutomationsForAdmin(
        Expression<Func<AutomationRule, bool>>[]? filter = null,
        Expression<Func<AutomationRule, object>>[]? includes = null,
        int pageNumber = 0,
        int pageSize = 10);

}