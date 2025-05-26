using System;

namespace Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;

public class AddBudgetDto
{
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public string Name { get; set; }
    public string? CategoryName { get; set; }
    public int? SubCategoryId { get; set; }
    public decimal BudgetAmount { get; set; } // PlannedAmount
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    //public decimal SpentAmount { get; set; }
    //public decimal RemainingAmount { get; set; }
    public bool RolloverUnspent { get; set; } = false; // only if it valid
    //public int BudgetTypeId { get; set; } = 0;
    public int? DayOfMonth { get; set; }
    public int? DayOfWeek { get; set; }
    
    // Name - Amount - sub Category ID - Category Name - Start Date + End - Day of Week - Day of Month
    // 
}