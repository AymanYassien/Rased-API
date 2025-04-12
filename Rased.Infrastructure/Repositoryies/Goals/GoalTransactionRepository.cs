using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Repositoryies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Goals
{
    public class GoalTransactionRepository : Repository<GoalTransaction, int>, IGoalTransactionRepository
    {
        public GoalTransactionRepository(RasedDbContext context) : base(context)
        {
        }
    }
}
