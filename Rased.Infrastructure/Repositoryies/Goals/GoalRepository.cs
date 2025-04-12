using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.Savings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Goals
{
    public class GoalRepository : Repository<Goal, int>, IGoalRepository
    {
        public GoalRepository(RasedDbContext context) : base(context)
        {
        }
    }
}
