using api5.Rased_API.Rased.Business.Services.Incomes;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Savings;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Services.ExpenseService;
using Rased.Business.Services.Goals;
using Rased.Business.Services.Savings;
using Rased.Business.Services.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.RecommendSystem
{
    public class AiRecommendationService : IAiRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIncomeService _incomeService;
        private readonly IExpenseService _expenseService;
        private readonly IBudgetService _budgetService;
        private readonly IGoalService _goalService;
        private readonly ISavingService _savingService;
        private readonly ITransactionService _transferService;

        public AiRecommendationService( IUnitOfWork unitOfWork , IIncomeService incomeService , IExpenseService expenseService , IBudgetService budgetService , 
            IGoalService goalService , ISavingService savingService ,ITransactionService transactionService)
        {
            _unitOfWork = unitOfWork;
            _incomeService = incomeService;
            _expenseService = expenseService;
            _budgetService = budgetService;
            _goalService = goalService;
            _savingService = savingService;
            _transferService = transactionService;
        }



        public async Task<WalletDataForAI> CollectWalletDataAsync(int walletId, string userId)
        {
            var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);
            if (wallet == null)
                throw new ArgumentException("Wallet not found.");

            if (wallet.CreatorId != userId) 
                throw new UnauthorizedAccessException("You don't own this wallet.");

            var incomes = await _incomeService.GetUserIncomesByWalletId(walletId);
            var expenses = await _expenseService.GetUserExpensesByWalletId(walletId);
            var budgets = await _budgetService.GetBudgetsByWalletIdAsync(walletId);
            var goals = await _goalService.GetGoalsByWalletIdAndUserIdAsync(walletId, userId);
            var savings = await _savingService.GetAllSavingsByWalletAsync(userId, walletId);
            var transfers = await _transferService.GetTransactionsBySenderIdAsync(userId, walletId);

            return new WalletDataForAI
            {
                WalletId = walletId,
                WalletName = wallet.Name,
                Incomes = incomes.Data as List<IncomeDto> ?? new List<IncomeDto>(),
                Expenses = expenses.Data as List<ExpenseDto> ?? new List<ExpenseDto>(),
                Budgets = budgets.Data as List<BudgetDto> ?? new List<BudgetDto>(),
                Goals = goals.Data?.ToList() ?? new List<ReadGoalDto>(),
                Savings = savings.Data?.ToList() ?? new List<ReadSavingDto>(),
                Transfers = transfers.Data ?? new List<ReadTransactionForSenderDto>()
            };
        }







        private string BuildPrompt(WalletDataForAI data)
        {
            var sb = new StringBuilder();

            sb.AppendLine($"عندي محفظة مالية اسمها: {data.WalletName}.");
            sb.AppendLine();

            // Incomes
            sb.AppendLine($"📥 الدخل (عدد {data.Incomes.Count}):");
            foreach (var income in data.Incomes)
            {
                sb.AppendLine($"- مبلغ: {income.Amount} جنيه، التصنيف: {income.CategoryName} / {income.SubCategoryId}، التاريخ: {income.CreatedDate.ToShortDateString()}");
            }
            sb.AppendLine();

            // Expenses
            sb.AppendLine($"📤 المصروفات (عدد {data.Expenses.Count}):");
            foreach (var expense in data.Expenses)
            {
                sb.AppendLine($"- مبلغ: {expense.Amount} جنيه، التصنيف: {expense.CategoryName} / {expense.SubCategoryId}، التاريخ: {expense.Date.ToShortDateString()}");
            }
            sb.AppendLine();

            // Budgets
            sb.AppendLine($"📊 الميزانيات (عدد {data.Budgets.Count}):");
            foreach (var budget in data.Budgets)
            {
                sb.AppendLine($"- تصنيف: {budget.CategoryName} / {budget.SubCategoryId}، المبلغ: {budget.BudgetAmount} جنيه");
            }
            sb.AppendLine();

            // Goals
            sb.AppendLine($"🎯 الأهداف المالية (عدد {data.Goals.Count}):");
            foreach (var goal in data.Goals)
            {
                sb.AppendLine($"- {goal.Name}: الهدف {goal.TargetAmount}، الحالي {goal.CurrentAmount}");
            }
            sb.AppendLine();

            // Savings
            sb.AppendLine($"💰 المدخرات (عدد {data.Savings.Count}):");
            foreach (var saving in data.Savings)
            {
                sb.AppendLine($"- {saving.Name}: {saving.TotalAmount} جنيه");
            }
            sb.AppendLine();

            // Transfers
            sb.AppendLine($"🔄 التحويلات (عدد {data.Transfers.Count()}):");
            foreach (var transfer in data.Transfers)
            {
                sb.AppendLine($"- حوّلت {transfer.Amount} جنيه لـ {transfer.ReceiverId} في {transfer.CreatedAt.ToShortDateString()}");
            }
            sb.AppendLine();

            sb.AppendLine("بناءً على البيانات دي، إديني توصيات مالية لتحسين إدارتي لأموالي الشهرية.");

            return sb.ToString();
        }






    }
}
