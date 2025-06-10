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
        public List<BudgetDto> Budgets { get; set; }
        public List<ReadGoalDto> Goals { get; set; }
        public List<ReadSavingDto> Savings { get; set; }
        public List<ReadTransactionForSenderDto> Transfers { get; set; }
    }



}
