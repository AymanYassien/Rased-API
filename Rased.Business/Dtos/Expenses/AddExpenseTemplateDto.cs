namespace Rased.Business.Dtos;

public class AddExpenseTemplateDto
{
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public string Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int? PaymentMethodId { get; set; } = 1;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? DayOfMonth { get; set; }
    public int? DayOfWeek { get; set; }
    
}


// insert automation role first then assign its id to Expense Template