using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IServices ..
        // IAuthService RasedAuth { get; }
        // ....
        
        public IExpensesRepository Expenses { get; }                
        public IIncomeRepository Income { get; }
        public IBudgetRepository Budget { get; }

        // Commit Changes
        Task<int> CommitChangesAsync(); // return Affects Row!
    }
}
