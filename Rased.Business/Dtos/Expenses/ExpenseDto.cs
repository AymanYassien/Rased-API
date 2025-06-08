namespace Rased.Business.Dtos;

public class ExpenseDto
{
    public int ExpenseId { get; set; }
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int? SubCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? SubCategoryName { get; set; }
    public DateTime Date { get; set; }
    public int? PaymentMethodId { get; set; }
    public bool IsAutomated { get; set; } 
    public int? RelatedBudgetId { get; set; }
    public string? RelatedBudgetName { get; set; }
    

}