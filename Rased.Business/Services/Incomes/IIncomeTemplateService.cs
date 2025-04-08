using System.Linq.Expressions;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public interface IIncomeTemplateService
{
    // Basic CRUD
    public Task<ApiResponse<object>> GetUserIncomeTemplatesByWalletId(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    public Task<ApiResponse<object>> GetUserIncomeTemplate(int walletId, int incomeTemplateId, bool isShared = false );
    public Task<ApiResponse<object>> AddUserIncomeTemplate(AddIncomeTemplateDto newIncomeTemplate);
    public Task<ApiResponse<object>> UpdateUserIncomeTemplate(int walletId, int incomeTemplateId, UpdateIncomeTemplateDto updateIncomeTemplateDto , bool isShared = false);
    public Task<ApiResponse<object>> DeleteUserIncomeTemplate(int incomeTemplateId);
    
    // Count

    public Task<ApiResponse<object>> CountIncomeTemplate(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, 
        bool isShared = false);
    
    
    // Calculated  - maybe not heavy use those points - 
    public Task<ApiResponse<object>> CalculateTotalIncomeTemplateAmount(int walletId, bool isShared = false, Expression<Func<IncomeTemplate, bool>>[]? filter = null);
    
    // Filter Income 
    
    // For Admin 
    public Task<ApiResponse<object>> GetAllIncomeTemplatesForAdmin(bool isShared = false, Expression<Func<IncomeTemplate, bool>>[]? filter = null);
    
}