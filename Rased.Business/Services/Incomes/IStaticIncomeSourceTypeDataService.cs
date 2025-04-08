using System.Linq.Expressions;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure;

namespace api5.Rased_API.Rased.Business.Services.Incomes;

public interface IStaticIncomeSourceTypeDataService
{
    public Task<ApiResponse<object>> GetAllIncomeSources(Expression<Func<StaticIncomeSourceTypeData, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10);
    public Task<ApiResponse<object>> GetIncomeSourcesById(int incomeSourceTypeId);
    public Task<ApiResponse<object>> AddIncomeSources(StaticIncomeSourceTypeDataDto incomeSourceType);
    public Task<ApiResponse<object>> UpdateIncomeSources(int incomeSourceTypeId, StaticIncomeSourceTypeDataDto update);
    public Task<ApiResponse<object>> DeleteIncomeSources(int incomeSourceTypeId);
    
}