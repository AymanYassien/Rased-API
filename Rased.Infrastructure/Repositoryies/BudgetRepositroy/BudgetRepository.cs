using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.BudgetRepositroy;

public class BudgetRepository : Repository<Budget, int>, IBudgetRepository
{
    private readonly RasedDbContext _context;
    
    public BudgetRepository(RasedDbContext context) : base(context)
        => _context = context;
}