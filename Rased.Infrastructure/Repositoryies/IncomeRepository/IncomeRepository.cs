using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public class IncomeRepository : Repository<Income, int>, IIncomeRepository
{
    private readonly RasedDbContext _context;
    
    public IncomeRepository(RasedDbContext context) : base(context)
        => _context = context;
    
    
}