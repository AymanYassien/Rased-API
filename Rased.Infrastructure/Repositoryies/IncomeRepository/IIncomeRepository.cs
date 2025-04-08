using System.Linq.Expressions;
using Rased.Infrastructure;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.IncomeRepository;

public interface IIncomeRepository : IRepository_Test<Income, int>
{
    Task<IQueryable<Income>> GetUserIncomesByWalletIdAsync(int walletId, bool isShared = false);
    
    Task<IQueryable<Income>> GetUserIncomesByWalletIdAsync(int walletId, Expression<Func<Income, bool>>[]? filter = null, int pageNumber = 0, int pageSize = 10,  bool isShared = false);
    Task<Income> GetUserIncomesAsync(int walletId, int incomeId, bool isShared = false);
    
    Task<decimal> CalculateTotalIncomesAmountAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null);
    Task<decimal> CalculateTotalIncomesAmountForLastWeekAsync(int walletId, bool isShared = false ,Expression<Func<Income, bool>>[]? filter = null);
    Task<decimal> CalculateTotalIncomesAmountForLastMonthAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null);
    Task<decimal> CalculateTotalIncomesAmountForLastYearAsync(int walletId, bool isShared = false, Expression<Func<Income, bool>>[]? filter = null);
    Task<decimal> CalculateTotalIncomesAmountForSpecificPeriodAsync(int walletId, DateTime startDateTime, DateTime endDateTime, Expression<Func<Income, bool>>[]? filter = null, bool isShared = false);

}