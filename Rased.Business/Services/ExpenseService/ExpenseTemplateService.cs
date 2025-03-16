using System.Linq.Expressions;
using System.Net;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Business.Services.ExpenseService;

public class ExpenseTemplateService : IExpenseTemplateService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public ExpenseTemplateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
         
    }
    public async Task<ApiResponse<object>> GetUserExpenseTemplatesByWalletId(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        // if (1 > walletId)
        //     return _response.Response(false, null, "",
        //         "Bad Request ",  HttpStatusCode.BadRequest);
        //
        // IQueryable<ExpenseTemplate> res = await _unitOfWork.ExpenseTemplates.GetUserExpensesTemplateByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        //
        // if (res == null)
        //     return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);
        //
        // //IQueryable < ExpenseTemplateDto > newResult = MapToExpenseTemplateDto(res);
        //
        // //return  _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

        throw new NotImplementedException();

    }
    
    public async Task<ApiResponse<object>> GetUserExpenseTemplate(int walletId, int expenseTemplateId, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.ExpenseTemplates.GetUserExpenseAsync(walletId, expenseTemplateId, isShared);
        
        if (res == null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = MapToExpenseTemplateDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);


    }

    public async Task<ApiResponse<object>> AddUserExpenseTemplate(AddExpenseTemplateDto newExpenseTemplate)
    {
        throw new NotImplementedException();
    }

    public async Task<ApiResponse<object>> UpdateUserExpenseTemplate(int expenseTemplateId, UpdateExpenseTemplateDto updateExpenseTemplateDto)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> DeleteUserExpenseTemplate(int expenseTemplateId)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> CountExpensesTemplate(int walletId, Expression<Func<Expense, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmount(int walletId, bool isShared = false, Expression<Func<Expense, bool>>[]? filter = null)
    {
        throw new NotImplementedException();
    }
    
    public Task<ApiResponse<object>> GetAllExpensesTemplatesForAdmin(Expression<Func<Expense, bool>>[]? filter = null, Expression<Func<Expense, object>>[]? includes = null, int pageNumber = 0,
        int pageSize = 10)
    {
        throw new NotImplementedException();
    }



    private async Task<AutomationRule> GetAutomationRuleById(int automationRoleId)
    {
        return await _unitOfWork.AutomationRules.GetByIdAsync(automationRoleId);
    }
    private async Task<ExpenseTemplateDto> MapToExpenseTemplateDto(ExpenseTemplate expenseTemplate)
    {
        var automation = await GetAutomationRuleById(expenseTemplate.AutomationRuleId);
        return new ExpenseTemplateDto()
        {
            TemplateId = expenseTemplate.TemplateId,
            WalletId = expenseTemplate.WalletId,
            SharedWalletId = expenseTemplate.SharedWalletId,
            AutomationRuleId = expenseTemplate.AutomationRuleId,
            Name = expenseTemplate.Name,
            Amount = expenseTemplate.Amount,
            CategoryName = expenseTemplate.CategoryName,
            SubCategoryId = expenseTemplate.SubCategoryId,
            IsNeedApprovalWhenAutoAdd = expenseTemplate.IsNeedApprovalWhenAutoAdd,
            PaymentMethodId = expenseTemplate.PaymentMethodId,
            Description = expenseTemplate.Description,
            
            IsActive = automation.IsActive,
            StartDate = automation.StartDate,
            EndDate = automation.EndDate,
            DayOfMonth = automation.DayOfMonth,
            DayOfWeek = automation.DayOfWeek
            
        };
    }
    
}