using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Repositoryies.Utility;

namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IServices ..
        // IAuthService RasedAuth { get; }
        // ....
        
        public IExpensesRepository Expenses { get; }                
        public IExpenseTemplateRepository ExpenseTemplates{ get; }                
        public IAutomationRuleRepository AutomationRules{ get; }                
        public IIncomeRepository Income { get; }
        public IBudgetRepository Budget { get; }

        // Commit Changes
        Task<int> CommitChangesAsync(); // return Affects Row!
    }
}
