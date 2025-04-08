using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public class StaticIncomeSourceTypeDataRepository : Repository_Test<StaticIncomeSourceTypeData, int>, IStaticIncomeSourceTypeDataRepository
{
    public StaticIncomeSourceTypeDataRepository(RasedDbContext context) : base(context)
    {
    }
}