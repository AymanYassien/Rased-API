using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Savings
{
    public class SavingRepository : Repository<Saving, int>, ISavingRepository
    {
        public SavingRepository(RasedDbContext context) : base(context)
        {
        }
    }
}
