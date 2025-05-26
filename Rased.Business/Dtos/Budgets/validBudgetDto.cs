using System;
using System.Collections.Generic;
using Rased.Business.Dtos;

namespace Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;

public class validBudgetDto
{
    public int BudgetId { get; set; }
    public int? WalletId { get; set; }
    public int? SharedWalletId { get; set; }
    public string Name { get; set; }
    public string? CategoryName { get; set; }
    public int? SubCategoryId { get; set; }
    public decimal BudgetAmount { get; set; } // PlannedAmount
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public bool RolloverUnspent { get; set; }
    public int  BudgetTypeId { get; set; }
    public int? DayOfMonth { get; set; }
    public int? DayOfWeek { get; set; }
    public List<ExpenseDto> relatedExpenses { get; set; } = new List<ExpenseDto>();
    
    public string subCategoryName { get; set; }
    public int CategoryId { get; set; }
}