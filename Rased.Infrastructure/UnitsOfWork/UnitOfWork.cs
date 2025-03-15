using Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;
using Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;
using Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Infrastructure.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // All IRepositoryies to be injected 
        private readonly RasedDbContext _context;
        // ....
        public IExpensesRepository Expenses { get; private set; }                
        public IIncomeRepository   Income   { get; private set; }
        public IBudgetRepository   Budget   { get; private set; }



        // All System IRepositoryies to be instantiated by the constructor
     
        public ISavingRepository SavingRepository { get; private set; }

        public IRepository<Goal, int> GoalRepository { get; private set; }


        // Constructor to inject all Repositoryies
        public UnitOfWork(RasedDbContext context)
        {
            _context = context;
            
            Expenses = new ExpenseRepository(_context);
            Income   = new IncomeRepository(_context);
            Budget   = new BudgetRepository(_context);
            SavingRepository = new SavingRepository(context);
            GoalRepository = new Repository<Goal, int>(context);
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
