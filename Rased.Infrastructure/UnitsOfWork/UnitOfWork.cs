using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Utility;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Infrastructure.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // All Services to be injected 
        private readonly RasedDbContext _context;
        // ....
        public IExpensesRepository Expenses { get; private set; }
        public IExpenseTemplateRepository ExpenseTemplates { get; private set; }
        public IAutomationRuleRepository AutomationRules { get; private set; }
        public IIncomeRepository   Income   { get; private set; }
        public IBudgetRepository   Budget   { get; private set; }


        // All System Services to be instantiated by the constructor
        // public IAuthService RasedAuth { get; private set; }
        // ....

        // Constructor to inject all services and instantiate all system services
        public UnitOfWork(RasedDbContext context)
        {
            _context = context;
            
            Expenses = new ExpenseRepository(_context);
            ExpenseTemplates = new ExpenseTemplateRepository(_context);
            AutomationRules = new AutomationRuleRepository(_context);
            Income   = new IncomeRepository(_context);
            Budget   = new BudgetRepository(_context);
        }

        // Save All System Changes and return the number of affected rows
        public async Task<int> CommitChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        // Dispose the context to free memory space
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
