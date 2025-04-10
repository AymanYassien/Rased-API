using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.Repositoryies.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Repositoryies.Savings
{
    public interface ISavingRepository : IRepository<Saving , int>
    {
        
    }
}
