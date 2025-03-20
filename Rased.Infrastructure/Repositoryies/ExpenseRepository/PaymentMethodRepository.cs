using Rased.Business.Dtos;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public class PaymentMethodRepository : Repository_Test<StaticPaymentMethodsData, int>, IPaymentMethodRepository
{
    public PaymentMethodRepository(RasedDbContext context) : base(context)
    {
    }
}