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



        // All System IRepositoryies to be instantiated by the constructor
     
        public ISavingRepository SavingRepository { get; private set; }

        public IRepository<Goal, int> GoalRepository { get; private set; }


        // Constructor to inject all Repositoryies
        public UnitOfWork(RasedDbContext context)
        {
            _context = context;
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
