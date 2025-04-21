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
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        filter = new Expression<Func<ExpenseTemplate, bool>>[]
        {
            a => a.AutomationRule.AutomationRuleId == a.AutomationRuleId
        };
        
        IQueryable<ExpenseTemplate> res = await _unitOfWork.ExpenseTemplates.GetUserExpensesTemplateByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if (!res.Any())
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable<ExpenseTemplateDto> newResult = MapToExpenseTemplatesDto(res);
        
        return  _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

    }
    
    public async Task<ApiResponse<object>> GetUserExpenseTemplate(int expenseTemplateId)
    {
        if (1 > expenseTemplateId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.ExpenseTemplates.GetByIdAsync( expenseTemplateId);
        
        if (res == null)
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = await MapToExpenseTemplateDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);


    }

    public async Task<ApiResponse<object>> AddUserExpenseTemplate(AddExpenseTemplateDto newExpenseTemplate)
    {
        if (!IsAddDtoValid(newExpenseTemplate, out var errorMessage))
        {
            return _response.Response(false, newExpenseTemplate, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            // Add the automation rule first
            var automationRule = FillAutomationByAddDto(newExpenseTemplate);
            await _unitOfWork.AutomationRules.AddAsync(automationRule);
            await _unitOfWork.CommitChangesAsync();  // Commit to get the generated ID

// Now create the expense template with the valid AutomationRuleId
            var expenseTemplate = FillExpenseTemplateByAddDto(newExpenseTemplate, automationRule.AutomationRuleId);
            await _unitOfWork.ExpenseTemplates.AddAsync(expenseTemplate);
            await _unitOfWork.CommitChangesAsync();  // Commit the expense template

            var newDTO = await MapToExpenseTemplateDto(expenseTemplate, automationRule);

            return _response.Response(true, newDTO, $"Success add Expense with id: {newDTO.TemplateId}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newExpenseTemplate, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
        
        
    }

    public async Task<ApiResponse<object>> UpdateUserExpenseTemplate(int expenseTemplateId, UpdateExpenseTemplateDto updateExpenseTemplateDto )
    {
        if (!IsUpdateDtoValid(updateExpenseTemplateDto, out var errorMessage))
        {
            return _response.Response(false, updateExpenseTemplateDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            var expenseTemplate =
                await _unitOfWork.ExpenseTemplates.GetByIdAsync(expenseTemplateId);

            if (expenseTemplate.TemplateId == expenseTemplateId && expenseTemplate.AutomationRuleId == updateExpenseTemplateDto.AutomationRuleId)
            {
                var automationRule = await _unitOfWork.AutomationRules.GetByIdAsync(updateExpenseTemplateDto.AutomationRuleId);
                if (automationRule is not null)
                {
                    var entity = UpdateAutomationRule(updateExpenseTemplateDto, automationRule);
                    _unitOfWork.AutomationRules.Update(entity);
                    await _unitOfWork.CommitChangesAsync();
                } 
                var fullEntity = UpdateExpenseTemplate(expenseTemplate, updateExpenseTemplateDto);
                _unitOfWork.ExpenseTemplates.Update(fullEntity);
                await _unitOfWork.CommitChangesAsync();
                
                return _response.Response(true, null, $"Success Update Expense Template with id: {expenseTemplate.TemplateId}", $"",
                    HttpStatusCode.OK);
            }
            else
                return _response.Response(false, updateExpenseTemplateDto, "", $"Bad Request",
                    HttpStatusCode.BadRequest);
            
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateExpenseTemplateDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> DeleteUserExpenseTemplate(int expenseTemplateId)
    {
        if (1 > expenseTemplateId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.ExpenseTemplates.RemoveById(expenseTemplateId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);
        
    }

    public async Task<ApiResponse<object>> CountExpensesTemplate(int walletId, Expression<Func<ExpenseTemplate, bool>>[]? filter = null, 
        bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.ExpenseTemplates.CountExpensesTemplateAsync(walletId, null);
        
        if ( res == 0)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(false, res, "This is Number of Expenses Templates", "",  HttpStatusCode.OK);
    }

    public async Task<ApiResponse<object>> CalculateTotalExpensesTemplateAmount(int walletId, bool isShared = false, Expression<Func<ExpenseTemplate, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.ExpenseTemplates.CalculateTotalExpensesTemplateAmountAsync(walletId,isShared);
        
        if ( res == 0)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(false, res, "This is Total Amount of Expenses Templates", "",  HttpStatusCode.OK);

    }
    
    public async Task<ApiResponse<object>> GetAllExpensesTemplatesForAdmin( Expression<Func<ExpenseTemplate, bool>>[]? filter = null)
    {
        var res = await _unitOfWork.ExpenseTemplates.GetAllAsync(filter);
        
        if (!res.Any())
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(false, res, "", "",  HttpStatusCode.OK);

    }
    
    private async Task<AutomationRule> GetAutomationRuleById(int automationRoleId)
    {
        return await _unitOfWork.AutomationRules.GetByIdAsync(automationRoleId);
    }

    private  ExpenseTemplate UpdateExpenseTemplate(ExpenseTemplate expenseTemplate, UpdateExpenseTemplateDto dto )
    {
        // update some fields

        expenseTemplate.Name = dto.Name;
        expenseTemplate.SubCategoryId = dto.SubCategoryId;
        expenseTemplate.CategoryName = dto.CategoryName;
        expenseTemplate.Amount = dto.Amount;
        expenseTemplate.Description = dto.Description;
        expenseTemplate.PaymentMethodId = dto.PaymentMethodId;

        return expenseTemplate;
    }
    
    private  AutomationRule UpdateAutomationRule(UpdateExpenseTemplateDto dto, AutomationRule automationRule)
    {
        automationRule.Title = dto.Name;
        automationRule.Description = dto.Description;
        automationRule.StartDate = dto.StartDate;
        automationRule.EndDate = dto.EndDate;
        automationRule.IsActive = dto.IsActive;
        automationRule.DayOfMonth = dto.DayOfMonth;
        automationRule.DayOfWeek = dto.DayOfWeek;

        return automationRule;
    }
    
    private async Task<ExpenseTemplateDto> MapToExpenseTemplateDto(ExpenseTemplate expenseTemplate, AutomationRule automation)
    {
        
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
    
    private  IQueryable<ExpenseTemplateDto> MapToExpenseTemplatesDto(IQueryable<ExpenseTemplate> expenseTemplates)
    {
        
        return  expenseTemplates.Select(expenseTemplate => new ExpenseTemplateDto
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
            
            IsActive = expenseTemplate.AutomationRule.IsActive,
            StartDate = expenseTemplate.AutomationRule.StartDate,
            EndDate = expenseTemplate.AutomationRule.EndDate,
            DayOfMonth = expenseTemplate.AutomationRule.DayOfMonth,
            DayOfWeek = expenseTemplate.AutomationRule.DayOfWeek
            
        });
    }
    
    private bool IsAddDtoValid(AddExpenseTemplateDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.Name))
        {
            errorMessage = "Title is required.";
            return false;
        }
        if (dto.Name.Length > 50)
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
    
        if ( dto.StartDate > dto.EndDate )
        {
            errorMessage = "Ensure End Date.";
            return false;
        }

        // 6. DayOfMonth: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 1-31)
        if (dto.DayOfMonth.HasValue && dto.DayOfMonth < 0 && dto.DayOfMonth > 29)
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
        

        if (dto.CategoryName?.Length > 50)
        {
            errorMessage = "CategoryName cannot exceed 50 characters.";
            return false;
        }
        
        if (dto.Amount == default(decimal))
        {
            errorMessage = "Amount is required.";
            return false;
        }
        if (dto.Amount <= 0)
        {
            errorMessage = "Amount must be greater than 0.";
            return false;
        }
        if (dto.Amount != decimal.Round(dto.Amount, 2) || dto.Amount > 999999.99m)
        {
            errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
            return false;
        }

        if (dto.SubCategoryId != null && dto.SubCategoryId < 1 )
        {
            errorMessage = "Sub Category Id must > zero";
            return false;
        }
        
        if (dto.PaymentMethodId != null && dto.PaymentMethodId < 1 )
        {
            errorMessage = "Payment Method Id must > zero";
            return false;
        }
        
    

        return true;
    }
    
    private bool IsUpdateDtoValid(UpdateExpenseTemplateDto dto, out string errorMessage)
    {
        errorMessage = string.Empty;

        // 1. Title: Required, MaxLength(50)
        if (string.IsNullOrEmpty(dto.Name))
        {
            errorMessage = "Title is required.";
            return false;
        }
        if (dto.Name.Length > 50)
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
    
        if ( dto.StartDate > dto.EndDate )
        {
            errorMessage = "Ensure End Date.";
            return false;
        }

        // 6. DayOfMonth: Optional, no specific range in Fluent API
        // Add custom range check if needed (e.g., 1-31)
        if (dto.DayOfMonth.HasValue && dto.DayOfMonth < 0 && dto.DayOfMonth > 29)
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
        

        if (dto.CategoryName?.Length > 50)
        {
            errorMessage = "CategoryName cannot exceed 50 characters.";
            return false;
        }
        
        if (dto.Amount == default(decimal))
        {
            errorMessage = "Amount is required.";
            return false;
        }
        if (dto.Amount <= 0)
        {
            errorMessage = "Amount must be greater than 0.";
            return false;
        }
        if (dto.Amount != decimal.Round(dto.Amount, 2) || dto.Amount > 999999.99m)
        {
            errorMessage = "Amount must have no more than 2 decimal places and fit within 8 digits (max 999999.99).";
            return false;
        }

        if (dto.SubCategoryId != null && dto.SubCategoryId < 1 )
        {
            errorMessage = "Sub Category Id must > zero";
            return false;
        }
        
        if (dto.PaymentMethodId != null && dto.PaymentMethodId < 1 )
        {
            errorMessage = "Payment Method Id must > zero";
            return false;
        }
        
        if (dto.IsActive != true && dto.IsActive != false )
        {
            errorMessage = "Must be True or False ";
            return false;
        }
        
        // not update isNeedApproval, templateId, automationID 
        
    

        return true;
    }

    private AutomationRule FillAutomationByAddDto(AddExpenseTemplateDto dto)
    {
        var automationRule = new AutomationRule();
        
        if (dto.WalletId != null)
            automationRule.WalletId = dto.WalletId;
        
        if (dto.SharedWalletId != null)
            automationRule.SharedWalletId = dto.SharedWalletId;
        
        if (dto.Description != null)
            automationRule.Description = dto.Description;
        
        if (dto.DayOfMonth != null)
            automationRule.DayOfMonth = dto.DayOfMonth;
        
        if (dto.DayOfWeek != null)
            automationRule.DayOfWeek = dto.DayOfWeek;

        automationRule.Title = dto.Name;
        automationRule.IsActive = true;
        automationRule.StartDate = dto.StartDate;
        automationRule.EndDate = dto.EndDate;
        
        return automationRule;
    } 
    
    private ExpenseTemplate FillExpenseTemplateByAddDto(AddExpenseTemplateDto dto, int automationRuleId)
    {
        var expenseTemplate = new ExpenseTemplate();
        
        if (dto.WalletId != null)
            expenseTemplate.WalletId = dto.WalletId;
        
        if (dto.SharedWalletId != null)
            expenseTemplate.SharedWalletId = dto.SharedWalletId;
        
        if (dto.Description != null)
            expenseTemplate.Description = dto.Description;
        
        if (dto.SubCategoryId != null)
            expenseTemplate.SubCategoryId = dto.SubCategoryId;
        
        if (dto.CategoryName != null)
            expenseTemplate.CategoryName = dto.CategoryName;
        
        if (dto.PaymentMethodId is > 0)
            expenseTemplate.PaymentMethodId = dto.PaymentMethodId;
        

        expenseTemplate.AutomationRuleId = automationRuleId;
        expenseTemplate.Name = dto.Name;
        expenseTemplate.Amount = dto.Amount;
        expenseTemplate.IsNeedApprovalWhenAutoAdd = false;
        
        
        return expenseTemplate;
    }

    private async Task<ExpenseTemplateDto> MapToExpenseTemplateDto(ExpenseTemplate expenseTemplate)
    {
        var rule = await GetAutomationRuleById(expenseTemplate.AutomationRuleId);
        return await MapToExpenseTemplateDto(expenseTemplate, rule);
        //return await MapToExpenseTemplateDto(expenseTemplate, await GetAutomationRuleById(expenseTemplate.AutomationRuleId));
    }
}