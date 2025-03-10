namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public interface IExpensesRepository
{
    Task<decimal> GetTotalExpensesAtMonthAsync(DateTime month);
}