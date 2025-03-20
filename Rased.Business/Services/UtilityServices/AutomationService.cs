using System.Linq.Expressions;
using System.Net;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased_API.Rased.Business;

public class AutomationService : IAutomationService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public AutomationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
         
    }

    public async Task<ApiResponse<object>> GetUserAutomationRulesByWalletIdAsync(int walletId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0,
        int pageSize = 10, bool isShared = false)
    {
        if (1 > walletId)
            return  _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        IQueryable<AutomationRule> res = await _unitOfWork.AutomationRules.GetUserAutomationRulesByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetAutomationRuleById(int automationId, Expression<Func<AutomationRule, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10)
    {
        if (1 > automationId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.AutomationRules.GetByIdAsync(automationId);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        
        
        return  _response.Response(true, res, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> AddAutomationRule(AutomationRule newAutomationRule)
    {
        if (!IsAutomationRuleDtoValid(newAutomationRule, out var errorMessage))
        {
            return _response.Response(false, newAutomationRule, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            await _unitOfWork.AutomationRules.AddAsync(newAutomationRule);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newAutomationRule, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, newAutomationRule, $"Success add Automation Rule with id: {newAutomationRule.AutomationRuleId}", $"",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<object>> UpdateUserAutomationRule(int ruleId, AutomationRule updateAutomationRule)
    {
        if (!IsAutomationRuleDtoValid(updateAutomationRule, out var errorMessage))
        {
            return _response.Response(false, updateAutomationRule, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        if (1 > ruleId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.Expenses.GetByIdAsync(ruleId);
        
        if (res == null)
            return _response.Response(false, null, "", $"Not Found Expense with id {ruleId}",  HttpStatusCode.NotFound);

        

        try
        {
            _unitOfWork.AutomationRules.Update(updateAutomationRule);
            await _unitOfWork.CommitChangesAsync();
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateAutomationRule, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        return _response.Response(true, updateAutomationRule, $"Success Update Automation Rule with id: {updateAutomationRule.AutomationRuleId}", $"",
            HttpStatusCode.Created);
    }

    public async Task<ApiResponse<object>> DeleteAutomationRule(int ruleId)
    {
        if (1 > ruleId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.AutomationRules.RemoveById(ruleId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> IsAutomationRoleValid(int automationRuleId)
    {
        if (1 > automationRuleId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var obj =  await _unitOfWork.AutomationRules.GetByIdAsync(automationRuleId);
        if (obj is null)
        {
            return _response.Response(false, null, "", "Not Found, or Nothing to Check",  HttpStatusCode.NotFound);
        }
        
        bool res =  await _unitOfWork.AutomationRules.IsAutomationRoleValid(automationRuleId); 
        return _response.Response(true, res, "Success", "",  HttpStatusCode.OK);
        
    }

    public async Task<ApiResponse<object>> DeactivateAutomationRole(int automationRuleId)
    {
        if (1 > automationRuleId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var obj =  await _unitOfWork.AutomationRules.GetByIdAsync(automationRuleId);
        if (obj is null)
        {
            return _response.Response(false, null, "", "Not Found, or Nothing to Check",  HttpStatusCode.NotFound);
        }

        obj.IsActive = false;
        _unitOfWork.CommitChangesAsync();
        return _response.Response(true, obj, "Success", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> CountAutomationRules(Expression<Func<AutomationRule, bool>>[]? filter = null)
    {
        var total = await _unitOfWork.AutomationRules.CountAutomationRules(filter);

        return _response.Response(true, total, "Success", "", HttpStatusCode.OK);
        
    }
    

    public async Task<ApiResponse<object>> GetAllAutomationsForAdmin(Expression<Func<AutomationRule, bool>>[]? filter = null, Expression<Func<AutomationRule, object>>[]? includes = null, int pageNumber = 0,
        int pageSize = 10)
    {
        var all = await _unitOfWork.AutomationRules.GetAllAsync(filter, includes, pageNumber, pageSize);
        return _response.Response(true, all, "Success", "", HttpStatusCode.OK);
    }
    
    
    private bool IsAutomationRuleDtoValid(AutomationRule dto, out string errorMessage)
{
    errorMessage = string.Empty;

    // 1. Title: Required, MaxLength(50)
    if (string.IsNullOrEmpty(dto.Title))
    {
        errorMessage = "Title is required.";
        return false;
    }
    if (dto.Title.Length > 50)
    {
        errorMessage = "Title cannot exceed 50 characters.";
        return false;
    }

    // 2. Description: MaxLength(200), optional
    if (dto.Description?.Length > 200)
    {
        errorMessage = "Description cannot exceed 200 characters.";
        return false;
    }

    
    // 4. StartDate: Required
    if (dto.StartDate == default(DateTime))
    {
        errorMessage = "StartDate is required.";
        return false;
    }

    // 5. EndDate: Required
    if (dto.EndDate == default(DateTime))
    {
        errorMessage = "EndDate is required.";
        return false;
    }
    
    if ( dto.StartDate < dto.EndDate)
    {
        errorMessage = "Ensure End Date.";
        return false;
    }

    // 6. DayOfMonth: Optional, no specific range in Fluent API
    // Add custom range check if needed (e.g., 1-31)
    if (dto.DayOfMonth.HasValue && dto.DayOfMonth is > 0 and < 29)
    {
        errorMessage = "DayOfMonth must be between 1 and 28.";
        return false;
    }

    // 7. DayOfWeek: Optional, no specific range in Fluent API
    // Add custom range check if needed (e.g., 0-6 for Sunday-Saturday)
    if (dto.DayOfWeek.HasValue && dto.DayOfWeek is > 0 and < 8)
    {
        errorMessage = "DayOfWeek must be between 0 and 7.";
        return false;
    }

    // 8. TriggerTypeId: Required
    if (dto.TriggerTypeId == default(int))
    {
        errorMessage = "TriggerTypeId is required.";
        return false;
    }

    

    return true;
}
}