namespace Rased.Infrastructure;

public class BudgetExpenseDto
{
    public string Budget { get; set; }
    public decimal Amount { get; set; }
}

public class ExpensesByBudgetDto
{
    public decimal Total { get; set; }
    public List<BudgetExpenseDto> Budgets { get; set; }
}