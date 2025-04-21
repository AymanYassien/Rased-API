using System.Linq.Expressions;
using System.Net;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public class IncomeTemplateService : IIncomeTemplateService
{
    private IUnitOfWork _unitOfWork;
    private ApiResponse<object> _response;
    
    public IncomeTemplateService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _response = new ApiResponse<object>();
    }
    
    public async Task<ApiResponse<object>> GetUserIncomeTemplatesByWalletId(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,
        bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);

        filter = new Expression<Func<IncomeTemplate, bool>>[]
        {
            a => a.AutomationRule.AutomationRuleId == a.AutomationRuleId
        };
        
        IQueryable<IncomeTemplate> res = await _unitOfWork.IncomeTemplate.GetUserIncomesTemplateByWalletIdAsync(walletId, filter, pageNumber, pageSize, isShared);
        
        if (!res.Any())
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        IQueryable<IncomeTemplateDto> newResult = MapToIncomeTemplatesDto(res);
        
        return  _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetTemplateById( int incomeTemplateId)
    {
        if (1 > incomeTemplateId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.IncomeTemplate.GetByIdAsync(incomeTemplateId);

        if (res == null) 
            return _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        var newResult = await MapToIncomeTemplateDto(res);
        
        return _response.Response(true, newResult, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> AddUserIncomeTemplate(AddIncomeTemplateDto newIncomeTemplate)
    {
        if (!IsAddDtoValid(newIncomeTemplate, out var errorMessage))
        {
            return _response.Response(false, newIncomeTemplate, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        
        try
        {
            var automationRule = FillAutomationByAddDto(newIncomeTemplate);
            await _unitOfWork.AutomationRules.AddAsync(automationRule);
            await _unitOfWork.CommitChangesAsync();
            
            var incomeTemplate = FillIncomeTemplateByAddDto(newIncomeTemplate, automationRule.AutomationRuleId);
            await _unitOfWork.IncomeTemplate.AddAsync(incomeTemplate);
            await _unitOfWork.CommitChangesAsync();

            var newDTO = await MapToIncomeTemplateDto(incomeTemplate, automationRule);
            
            return _response.Response(true, newDTO, $"Success add Income with id: {newDTO.TemplateId}", $"",
                HttpStatusCode.Created);
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, newIncomeTemplate, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> UpdateUserIncomeTemplate( int incomeTemplateId, UpdateIncomeTemplateDto updateIncomeTemplateDto)
    {
        if (!IsUpdateDtoValid(updateIncomeTemplateDto, out var errorMessage))
        {
            return _response.Response(false, updateIncomeTemplateDto, "", $"Bad Request, Error Messages : {errorMessage}",
                HttpStatusCode.BadRequest);
        }
        try
        {
            var incomeTemplate = await _unitOfWork.IncomeTemplate.GetByIdAsync( incomeTemplateId);

            if (incomeTemplate.IncomeTemplateId == incomeTemplateId && incomeTemplate.AutomationRuleId == updateIncomeTemplateDto.AutomationRuleId)
            {
                var automationRule = await _unitOfWork.AutomationRules.GetByIdAsync(updateIncomeTemplateDto.AutomationRuleId);
                if (automationRule is not null)
                {
                    UpdateAutomationRule(updateIncomeTemplateDto, automationRule);
                    _unitOfWork.AutomationRules.Update(automationRule);
                }

                UpdateIncomeTemplate(incomeTemplate, updateIncomeTemplateDto);
                _unitOfWork.IncomeTemplate.Update(incomeTemplate);
                await _unitOfWork.CommitChangesAsync();
                
                return _response.Response(true, null, $"Success Update Income Template with id: {incomeTemplate.IncomeTemplateId}", $"",
                    HttpStatusCode.NoContent);
            }
            else
                return _response.Response(false, updateIncomeTemplateDto, "", $"Bad Request",
                    HttpStatusCode.BadRequest);
            
        }
        catch (Exception ex)
        {
            
            return _response.Response(false, updateIncomeTemplateDto, "", $"Database constraint violation: {ex.InnerException?.Message}",
                HttpStatusCode.InternalServerError);
        }
    }

    public async Task<ApiResponse<object>> DeleteUserIncomeTemplate(int incomeTemplateId)
    {
        if (1 > incomeTemplateId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res =  _unitOfWork.IncomeTemplate.RemoveById(incomeTemplateId);
        if (res is false)
            return _response.Response(false, null, "", "Not Found, or fail to delete",  HttpStatusCode.NotFound);
        
        return _response.Response(true, null, "Success", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CountIncomeTemplate(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, bool isShared = false)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.IncomeTemplate.CountIncomesTemplateAsync(walletId, null, isShared);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(true, res, "This is Number of Income Templates", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> CalculateTotalIncomeTemplateAmount(int walletId, bool isShared = false, Expression<Func<IncomeTemplate, bool>>[]? filter = null)
    {
        if (1 > walletId)
            return _response.Response(false, null, "",
                "Bad Request ",  HttpStatusCode.BadRequest);
        
        var res = await _unitOfWork.IncomeTemplate.CalculateTotalIncomesTemplateAmountAsync(walletId, isShared);
        
        if (res == null)
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(true, res, "This is Number of Income Templates", "",  HttpStatusCode.OK);

    }

    public async Task<ApiResponse<object>> GetAllIncomeTemplatesForAdmin( Expression<Func<IncomeTemplate, bool>>[]? filter = null)
    {
        var res = await _unitOfWork.IncomeTemplate.GetAllAsync(filter);
        
        if (!res.Any())
            return  _response.Response(false, null, "", "Not Found",  HttpStatusCode.NotFound);

        return _response.Response(true, res, "This is Total Incomes Templates", "",  HttpStatusCode.OK);

    }
    
    private async Task<AutomationRule> GetAutomationRuleById(int automationRoleId)
    {
        return await _unitOfWork.AutomationRules.GetByIdAsync(automationRoleId);
    }

    private  void UpdateIncomeTemplate(IncomeTemplate incomeTemplate, UpdateIncomeTemplateDto dto )
    {
        // update some fields

        incomeTemplate.Name = dto.Name;
        incomeTemplate.SubCategoryId = dto.SubCategoryId;
        incomeTemplate.CategoryName = dto.CategoryName;
        incomeTemplate.Amount = dto.Amount;
        incomeTemplate.Description = dto.Description;
        incomeTemplate.IncomeSourceTypeId = dto.IncomeSourceTypeId;

        // return incomeTemplate;
    }
    
    private  void UpdateAutomationRule(UpdateIncomeTemplateDto dto, AutomationRule automationRule)
    {
        automationRule.Title = dto.Name;
        automationRule.Description = dto.Description;
        automationRule.StartDate = dto.StartDate;
        automationRule.EndDate = dto.EndDate;
        automationRule.IsActive = dto.IsActive;
        automationRule.DayOfMonth = dto.DayOfMonth;
        automationRule.DayOfWeek = dto.DayOfWeek;

        // return automationRule;
    }
    
    private async Task<IncomeTemplateDto> MapToIncomeTemplateDto(IncomeTemplate incomeTemplate, AutomationRule automation)
    {
        
        return new IncomeTemplateDto()
        {
            TemplateId = incomeTemplate.IncomeTemplateId,
            WalletId = incomeTemplate.WalletId,
            SharedWalletId = incomeTemplate.SharedWalletId,
            AutomationRuleId = incomeTemplate.AutomationRuleId,
            Name = incomeTemplate.Name,
            Amount = incomeTemplate.Amount,
            CategoryName = incomeTemplate.CategoryName,
            SubCategoryId = incomeTemplate.SubCategoryId,
            IsNeedApprovalWhenAutoAdd = incomeTemplate.IsNeedApprovalWhenAutoAdd,
            IncomeSourceTypeId = incomeTemplate.IncomeSourceTypeId,
            Description = incomeTemplate.Description,
            
            IsActive = automation.IsActive,
            StartDate = automation.StartDate,
            EndDate = automation.EndDate,
            DayOfMonth = automation.DayOfMonth,
            DayOfWeek = automation.DayOfWeek
            
        };
    }
    
    private  IQueryable<IncomeTemplateDto> MapToIncomeTemplatesDto(IQueryable<IncomeTemplate> incomeTemplates)
    {
        
        return  incomeTemplates.Select(incomeTemplate => new IncomeTemplateDto()
        {
            TemplateId = incomeTemplate.IncomeTemplateId,
            WalletId = incomeTemplate.WalletId,
            SharedWalletId = incomeTemplate.SharedWalletId,
            AutomationRuleId = incomeTemplate.AutomationRuleId,
            Name = incomeTemplate.Name,
            Amount = incomeTemplate.Amount,
            CategoryName = incomeTemplate.CategoryName,
            SubCategoryId = incomeTemplate.SubCategoryId,
            IsNeedApprovalWhenAutoAdd = incomeTemplate.IsNeedApprovalWhenAutoAdd,
            IncomeSourceTypeId = incomeTemplate.IncomeSourceTypeId,
            Description = incomeTemplate.Description,
            
            IsActive = incomeTemplate.AutomationRule.IsActive,
            StartDate = incomeTemplate.AutomationRule.StartDate,
            EndDate = incomeTemplate.AutomationRule.EndDate,
            DayOfMonth = incomeTemplate.AutomationRule.DayOfMonth,
            DayOfWeek = incomeTemplate.AutomationRule.DayOfWeek
            
        });
    }
    
    private bool IsAddDtoValid(AddIncomeTemplateDto dto, out string errorMessage)
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
        
        if (dto.IncomeSourceTypeId != null && dto.IncomeSourceTypeId < 1 )
        {
            errorMessage = "Income Source Type Id must > zero";
            return false;
        }
        
        return true;
    }
    
    private bool IsUpdateDtoValid(UpdateIncomeTemplateDto dto, out string errorMessage)
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
        
        if (dto.IncomeSourceTypeId != null && dto.IncomeSourceTypeId < 1 )
        {
            errorMessage = "Income Source Type Id must > zero";
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

    private AutomationRule FillAutomationByAddDto(AddIncomeTemplateDto dto)
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
    
    private IncomeTemplate FillIncomeTemplateByAddDto(AddIncomeTemplateDto dto, int automationRuleId)
    {
        var incomeTemplate = new IncomeTemplate();
        
        if (dto.WalletId != null)
            incomeTemplate.WalletId = dto.WalletId;
        
        if (dto.SharedWalletId != null)
            incomeTemplate.SharedWalletId = dto.SharedWalletId;
        
        if (dto.Description != null)
            incomeTemplate.Description = dto.Description;
        
        if (dto.SubCategoryId != null)
            incomeTemplate.SubCategoryId = dto.SubCategoryId;
        
        if (dto.CategoryName != null)
            incomeTemplate.CategoryName = dto.CategoryName;
        
        if (dto.IncomeSourceTypeId is > 0)
            incomeTemplate.IncomeSourceTypeId = dto.IncomeSourceTypeId;
        

        incomeTemplate.AutomationRuleId = automationRuleId;
        incomeTemplate.Name = dto.Name;
        incomeTemplate.Amount = dto.Amount;
        incomeTemplate.IsNeedApprovalWhenAutoAdd = false;
        
        
        return incomeTemplate;
    }

    private async Task<IncomeTemplateDto> MapToIncomeTemplateDto(IncomeTemplate incomeTemplate)
    {
        return await MapToIncomeTemplateDto(incomeTemplate, await GetAutomationRuleById(incomeTemplate.AutomationRuleId));
    }
    
}