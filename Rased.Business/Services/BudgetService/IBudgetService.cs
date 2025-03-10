using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;

namespace Rased_API.Rased.Business.Services.BudgetService;


public interface IBudgetService
{
    Task<BudgetDto> AddBudgetAsync(AddBudgetDto dto);
    Task<BudgetDto> UpdateBudgetAsync(UpdateBudgetDto dto);
    Task<BudgetDto> GetBudgetByIdAsync(int id);
    Task<IQueryable<BudgetDto>> GetAllBudgetsAsync(int pageNumber, int pageSize);
    Task<BudgetDto> GetActiveBudgetAsync(DateTime date);
    Task DeleteBudgetAsync(int id);
}
