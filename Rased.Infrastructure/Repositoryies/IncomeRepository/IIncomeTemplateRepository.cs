using System.Linq.Expressions;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public interface IIncomeTemplateRepository: IRepository_Test<IncomeTemplate, int>
{
    Task<IQueryable<IncomeTemplate>> GetUserIncomesTemplateByWalletIdAsync(int walletId, bool isShared = false);
    
    Task<IQueryable<IncomeTemplate>> GetUserIncomesTemplateByWalletIdAsync(int walletId, Expression<Func<IncomeTemplate, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    
    Task<IncomeTemplate> GetUserIncomesAsync(int walletId, int incomeTemplateId, bool isShared = false);
    
    Task<decimal> CalculateTotalIncomesTemplateAmountAsync(int walletId, bool isShared = false, Expression<Func<IncomeTemplate, bool>>[]? filter = null);
    
    Task<int> CountIncomesTemplateAsync(int walletId,Expression<Func<IncomeTemplate, bool>>[]? filter = null,  bool isShared = false);
    
    Task<int> CountIncomesTemplatesForAdminAsync(Expression<Func<IncomeTemplate, bool>>[]? filter = null,  bool isShared = false);

}