namespace Rased.Business.Dtos;

public class MonthlyExpenseSummary
{
    public int Year { get; set; }
    public int Month { get; set; }
    public decimal TotalAmount { get; set; }
}