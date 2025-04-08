namespace Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;

public class IncomeTemplateDto
{
    public int TemplateId { get; set; }
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public int AutomationRuleId { get; set; }
    public string Name { get; set; }
    public string? CategoryName { get; set; }
    public int? SubCategoryId { get; set; }
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int IncomeSourceTypeId { get; set; }
    public bool IsNeedApprovalWhenAutoAdd { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int? DayOfMonth { get; set; }
    public int? DayOfWeek { get; set; }
}