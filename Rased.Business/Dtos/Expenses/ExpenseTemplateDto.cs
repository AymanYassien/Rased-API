namespace Rased.Business.Dtos;

public class ExpenseTemplateDto
{
    public int TemplateId { get; set; }
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public int AutomationRuleId { get; set; }
    public string Name { get; set; }
    public int? SubCategoryId { get; set; }
    public string? CategoryName { get; set; }
    public decimal Amount { get; set; }
    public bool IsNeedApprovalWhenAutoAdd { get; set; }
    public string Description { get; set; }
    public int? PaymentMethodId { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? DayOfMonth { get; set; }
    public int? DayOfWeek { get; set; }
}