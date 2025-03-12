using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;

namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IRepositoryies ..
     public  ISavingRepository SavingRepository { get; }
     public IRepository<Goal , int> GoalRepository { get; }






        // Commit Changes
        Task<int> CommitChangesAsync();
    }
}
