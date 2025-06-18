using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Savings;
using Rased.Business.Dtos.Transfer;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Recomm
{
    public class BudgetRecommendationDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime GeneratedAt { get; set; }
        public bool IsRead { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class CreateBudgetRecommendationDto
    {
        public string UserId { get; set; }
        public int? WalletId { get; set; }
        public int? WalletGroupId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }


    public class WalletDataForAI
    {
        public int WalletId { get; set; }
        public string WalletName { get; set; }

        public List<IncomeDto> Incomes { get; set; }
        public List<ExpenseDto> Expenses { get; set; }
        public List<validBudgetDto> Budgets { get; set; }
        public List<ReadGoalDto> Goals { get; set; }
        public List<ReadSavingDto> Savings { get; set; }
        public List<ReadTransactionForSenderDto> Transfers { get; set; }
    }

    public class RecommendationTipDto
    {
        public string Tip { get; set; }
    }




    public class FinancialAnalysis
    {
        public decimal TotalIncome { get; set; }
        public decimal TotalExpenses { get; set; }
        public decimal NetIncome { get; set; }
        public decimal SavingsRate { get; set; }
        public decimal TotalSavings { get; set; }
        public decimal TotalTransfers { get; set; }
        public Dictionary<string, decimal> ExpensesByCategory { get; set; } = new();
        public List<BudgetUsageAnalysis> BudgetUsage { get; set; } = new();
        public List<GoalProgressAnalysis> GoalProgress { get; set; } = new();
    }

    public class BudgetUsageAnalysis
    {
        public string CategoryName { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal SpentAmount { get; set; }
        public decimal UsagePercentage { get; set; }
    }

    public class GoalProgressAnalysis
    {
        public string GoalName { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal CurrentAmount { get; set; }
        public decimal CompletionPercentage { get; set; }
        public decimal RemainingAmount { get; set; }
    }



}
