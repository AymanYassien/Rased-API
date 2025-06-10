namespace Rased_API.Rased.Infrastructure;

public class UpdateIncomeDto
{
    public int IncomeId { get; set; }
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public int IncomeSourceTypeId { get; set; }
    //public int? IncomeTemplateId { get; set; }
    //public DateTime CreatedDate { get; set; }
    public bool IsAutomated { get; set; } = false;
}