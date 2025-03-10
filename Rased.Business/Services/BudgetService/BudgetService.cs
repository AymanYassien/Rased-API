using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased_API.Rased.Business.Services.BudgetService;

public class BudgetService : IBudgetService
{
    private readonly IUnitOfWork _unitOfWork;
    public BudgetService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<decimal> GetMonthlyExpensesAsync(DateTime month)
    {
        // can ensure if month is 12, Validate date here .. etc
        
        return await _unitOfWork.Expenses.GetTotalExpensesAtMonthAsync(month);
    }

    public Task<BudgetDto> AddBudgetAsync(AddBudgetDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> UpdateBudgetAsync(UpdateBudgetDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> GetBudgetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<BudgetDto>> GetAllBudgetsAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<BudgetDto> GetActiveBudgetAsync(DateTime date)
    {
        throw new NotImplementedException();
    }

    public Task DeleteBudgetAsync(int id)
    {
        // var budget = await _unitOfWork.Budget.GetByIdAsync(id);
        // if (budget == null) throw new KeyNotFoundException("Budget not found");
        // _unitOfWork.Budget.Remove(budget);
        // await _unitOfWork.CommitChangesAsync();
        
        throw new NotImplementedException();
    }
}