using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased_API.Rased.Infrastructure.Repositoryies.ExpenseRepository;

public class ExpenseRepository : Repository<Expense, int>, IExpensesRepository
{
    private readonly RasedDbContext _context;
    
    public ExpenseRepository(RasedDbContext context) : base(context)
        => _context = context;
    
    public async Task<decimal> GetTotalExpensesAtMonthAsync(DateTime month)
    {
        var startOfMonth = new DateTime(month.Year, month.Month, 1);
        var endOfMonth = startOfMonth.AddMonths(1).AddDays(-1); // can handle date > 28
        return await _context.Expenses
            .Where(e => e.Date >= startOfMonth && e.Date <= endOfMonth)
            .SumAsync(e => e.Amount);
    }
}