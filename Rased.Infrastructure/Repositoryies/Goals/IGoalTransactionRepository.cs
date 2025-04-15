using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Repositoryies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Goals
{
    public interface IGoalTransactionRepository : IRepository<GoalTransaction , int>
    {
    }
}
