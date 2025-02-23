using Rased.Infrastructure.Data;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Infrastructure.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // All Services to be injected 
        private readonly RasedDbContext _context;
        // ....


        // All System Services to be instantiated by the constructor
        // public IAuthService RasedAuth { get; private set; }
        // ....

        // Constructor to inject all services and instantiate all system services
        public UnitOfWork(RasedDbContext context)
        {
            _context = context;
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
