using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public interface IPaymentMethodRepository : IRepository_Test<StaticPaymentMethodsData, int>
{
    
}