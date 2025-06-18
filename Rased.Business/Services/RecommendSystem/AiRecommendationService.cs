
using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.Extensions.Configuration;
using Rased.Business.Dtos.Goals;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Savings;
using Rased.Business.Dtos.Transfer;
using Rased.Business.Dtos;
using Rased.Business.Services.ExpenseService;
using Rased.Business.Services.Goals;
using Rased.Business.Services.RecommendSystem;
using Rased.Business.Services.Savings;
using Rased.Business.Services.Transfer;
using Rased.Infrastructure.UnitsOfWork;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased_API.Rased.Infrastructure.DTOs.BudgetDTO;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text;
using Rased.Infrastructure.Models.Recomm;

public class AiRecommendationService : IAiRecommendationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIncomeService _incomeService;
    private readonly IExpenseService _expenseService;
    private readonly IBudgetService _budgetService;
    private readonly IGoalService _goalService;
    private readonly ISavingService _savingService;
    private readonly ITransactionService _transferService;
    private readonly HttpClient _httpClient;
    private readonly string _geminiApiKey;
    private readonly string _geminiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

    public AiRecommendationService(
        IUnitOfWork unitOfWork,
        IIncomeService incomeService,
        IExpenseService expenseService,
        IBudgetService budgetService,
        IGoalService goalService,
        ISavingService savingService,
        ITransactionService transactionService,
        IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _incomeService = incomeService;
        _expenseService = expenseService;
        _budgetService = budgetService;
        _goalService = goalService;
        _savingService = savingService;
        _transferService = transactionService;
        _httpClient = new HttpClient();
        _geminiApiKey = configuration["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini API Key not found in configuration.");
    }

    public async Task<WalletDataForAI> CollectWalletDataAsync(int walletId, string userId)
    {
        var wallet = await _unitOfWork.Wallets.GetByIdAsync(walletId);
        if (wallet == null)
            throw new ArgumentException("Wallet not found.");

        if (wallet.CreatorId != userId)
            throw new UnauthorizedAccessException("You don't own this wallet.");

        var incomes = await _incomeService.GetUserIncomesByWalletIdToRecommendSystem(walletId);
        var expenses = await _expenseService.GetUserExpensesByWalletIdToRecommendSystem(walletId);
        var budgets = await _budgetService.GetBudgetsByWalletIdToRecommendSystemAsync(walletId);
        var goals = await _goalService.GetGoalsByWalletIdAndUserIdAsync(walletId, userId);
        var savings = await _savingService.GetAllSavingsByWalletAsync(userId, walletId);
        var transfers = await _transferService.GetTransactionsBySenderIdAsync(userId, walletId);

        return new WalletDataForAI
        {
            WalletId = walletId,
            WalletName = wallet.Name,
            Incomes = incomes.Data as List<IncomeDto> ?? new List<IncomeDto>(),
            Expenses = expenses.Data as List<ExpenseDto> ?? new List<ExpenseDto>(),
            Budgets = budgets.Data as List<validBudgetDto> ?? new List<validBudgetDto>(),
            Goals = goals.Data?.ToList() ?? new List<ReadGoalDto>(),
            Savings = savings.Data?.ToList() ?? new List<ReadSavingDto>(),
            Transfers = transfers.Data ?? new List<ReadTransactionForSenderDto>()
        };
    }

    private FinancialAnalysis AnalyzeData(WalletDataForAI data)
    {
        var totalIncome = data.Incomes.Sum(i => i.Amount);
        var totalExpenses = data.Expenses.Sum(e => e.Amount);
        var totalBudgets = data.Budgets.Sum(b => b.BudgetAmount);
        var totalSavings = data.Savings.Sum(s => s.TotalAmount);
        var totalTransfers = data.Transfers.Sum(t => t.Amount);

        var expensesByCategory = data.Expenses
            .GroupBy(e => e.SubCategoryName)
            .ToDictionary(g => g.Key, g => g.Sum(e => e.Amount));

        var budgetUsage = data.Budgets
            .Select(b => new BudgetUsageAnalysis
            {
                CategoryName = b.subCategoryName,
                BudgetAmount = b.BudgetAmount,
                SpentAmount = data.Expenses
                    .Where(e => e.SubCategoryName == b.subCategoryName)
                    .Sum(e => e.Amount),
                UsagePercentage = b.BudgetAmount > 0 ?
                    (data.Expenses.Where(e => e.SubCategoryName == b.subCategoryName).Sum(e => e.Amount) / b.BudgetAmount) * 100 : 0
            }).ToList();

        var goalProgress = data.Goals
            .Select(g => new GoalProgressAnalysis
            {
                GoalName = g.Name,
                TargetAmount = g.TargetAmount,
                CurrentAmount = g.CurrentAmount,
                CompletionPercentage = g.TargetAmount > 0 ? (g.CurrentAmount / g.TargetAmount) * 100 : 0,
                RemainingAmount = g.TargetAmount - g.CurrentAmount
            }).ToList();

        return new FinancialAnalysis
        {
            TotalIncome = totalIncome,
            TotalExpenses = totalExpenses,
            NetIncome = totalIncome - totalExpenses,
            SavingsRate = totalIncome > 0 ? (totalSavings / totalIncome) * 100 : 0,
            ExpensesByCategory = expensesByCategory,
            BudgetUsage = budgetUsage,
            GoalProgress = goalProgress,
            TotalSavings = totalSavings,
            TotalTransfers = totalTransfers
        };
    }

    private string BuildEnhancedPrompt(WalletDataForAI data)
    {
        var analysis = AnalyzeData(data);
        var sb = new StringBuilder();

        sb.AppendLine("أنت مستشار مالي خبير، أريد منك تحليل البيانات المالية التالية وتقديم توصيات مدروسة ومحددة.");
        sb.AppendLine();

        sb.AppendLine($"=== تحليل المحفظة المالية: {data.WalletName} ===");
        sb.AppendLine();

        // ملخص مالي
        sb.AppendLine("📊 **الملخص المالي:**");
        sb.AppendLine($"• إجمالي الدخل: {analysis.TotalIncome:F2} جنيه");
        sb.AppendLine($"• إجمالي المصروفات: {analysis.TotalExpenses:F2} جنيه");
        sb.AppendLine($"• صافي الدخل: {analysis.NetIncome:F2} جنيه");
        sb.AppendLine($"• معدل الادخار: {analysis.SavingsRate:F1}%");
        sb.AppendLine($"• إجمالي المدخرات: {analysis.TotalSavings:F2} جنيه");
        sb.AppendLine();

        // تحليل الميزانيات
        if (analysis.BudgetUsage.Any())
        {
            sb.AppendLine("💰 **تحليل الميزانيات:**");
            foreach (var budget in analysis.BudgetUsage)
            {
                var status = budget.UsagePercentage > 100 ? "تجاوز الميزانية" :
                           budget.UsagePercentage > 80 ? "قريب من الحد الأقصى" : "ضمن الحد المسموح";
                sb.AppendLine($"• {budget.CategoryName}: {budget.SpentAmount:F2}/{budget.BudgetAmount:F2} جنيه ({budget.UsagePercentage:F1}%) - {status}");
            }
            sb.AppendLine();
        }

        // تحليل الأهداف
        if (analysis.GoalProgress.Any())
        {
            sb.AppendLine("🎯 **تقدم الأهداف المالية:**");
            foreach (var goal in analysis.GoalProgress)
            {
                sb.AppendLine($"• {goal.GoalName}: {goal.CurrentAmount:F2}/{goal.TargetAmount:F2} جنيه ({goal.CompletionPercentage:F1}%) - متبقي {goal.RemainingAmount:F2} جنيه");
            }
            sb.AppendLine();
        }

        // توزيع المصروفات
        if (analysis.ExpensesByCategory.Any())
        {
            sb.AppendLine("📈 **توزيع المصروفات حسب التصنيف:**");
            foreach (var category in analysis.ExpensesByCategory.OrderByDescending(x => x.Value))
            {
                var percentage = analysis.TotalExpenses > 0 ? (category.Value / analysis.TotalExpenses) * 100 : 0;
                sb.AppendLine($"• {category.Key}: {category.Value:F2} جنيه ({percentage:F1}%)");
            }
            sb.AppendLine();
        }

        sb.AppendLine("=== المطلوب ===");
        sb.AppendLine("بناءً على التحليل أعلاه، أريد منك:");
        sb.AppendLine("1. تحديد نقاط القوة والضعف في إدارتي للأموال");
        sb.AppendLine("2. تقديم 5-7 توصيات محددة وقابلة للتطبيق");
        sb.AppendLine("3. اقتراح خطة عملية لتحسين الوضع المالي");
        sb.AppendLine();
        sb.AppendLine("**شروط الإجابة:**");
        sb.AppendLine("- اكتب كل توصية في فقرة منفصلة");
        sb.AppendLine("- ابدأ كل توصية برقم متبوع بنقطة (1. ، 2. ، إلخ)");
        sb.AppendLine("- اجعل كل توصية محددة وقابلة للقياس");
        sb.AppendLine("- استخدم أرقام من البيانات المذكورة");
        sb.AppendLine("- اكتب بأسلوب واضح ومباشر");

        return sb.ToString();
    }

    public async Task<List<RecommendationTipDto>> GetRecommendationAsync(int walletId, string userId)
    {
        try
        {
            var data = await CollectWalletDataAsync(walletId, userId);
            var prompt = BuildEnhancedPrompt(data);

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                generationConfig = new
                {
                    temperature = 0.7,
                    topK = 40,
                    topP = 0.95,
                    maxOutputTokens = 2048
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var endpoint = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_geminiApiKey}";

            var response = await _httpClient.PostAsync(endpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();

                if ((int)response.StatusCode == 429)
                {
                    return new List<RecommendationTipDto>
                    {
                        new RecommendationTipDto { Tip = "تم تجاوز الحد الأقصى من الطلبات، الرجاء المحاولة لاحقًا." }
                    };
                }

                throw new Exception($"Gemini request failed: {error}");
            }

            var responseJson = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseJson);

            var generatedText = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            var tips = ExtractEnhancedTips(generatedText ?? string.Empty);
          

            var currentDate = DateTime.UtcNow;
            var month = currentDate.Month;
            var year = currentDate.Year;

            // 🟢 تخزين التوصيات في قاعدة البيانات
            foreach (var tip in tips)
            {
                var recommendation = new BudgetRecommendation
                {
                    UserId = userId,
                    WalletId = walletId,
                    WalletGroupId =  null, // إذا كنت لا تستخدم WalletGroup هنا
                    Title = "توصية مالية",
                    Description = tip.Tip,
                    GeneratedAt = currentDate,
                    IsRead = false,
                    Month = month,
                    Year = year
                };

                await _unitOfWork.BudgetRecommendations.AddAsync(recommendation);
            }

            await _unitOfWork.CommitChangesAsync();

            return tips;
        }
        catch (Exception ex)
        {
            // Log the exception
            return new List<RecommendationTipDto>
            {
                new RecommendationTipDto { Tip = "حدث خطأ أثناء إنشاء التوصيات. يرجى المحاولة مرة أخرى لاحقاً." }
            };
        }
    }

    private List<RecommendationTipDto> ExtractEnhancedTips(string text)
    {
        var tips = new List<RecommendationTipDto>();

        if (string.IsNullOrWhiteSpace(text))
            return tips;

        // تنظيف النص من الرموز الزائدة
        text = text.Replace("**", "").Replace("*", "").Replace("#", "");

        // فصل التوصيات بناءً على الأرقام المتبوعة بنقطة
        var pattern = @"(?=^|\n)\s*(\d+)\.\s*(.+?)(?=\n\s*\d+\.\s*|\n\s*$|$)";
        var matches = Regex.Matches(text, pattern, RegexOptions.Multiline | RegexOptions.Singleline);

        foreach (Match match in matches)
        {
            if (match.Groups.Count >= 3)
            {
                var tipNumber = match.Groups[1].Value;
                var tipContent = match.Groups[2].Value.Trim();

                // تنظيف المحتوى
                tipContent = tipContent.Replace("\n", " ").Replace("\r", "");
                tipContent = Regex.Replace(tipContent, @"\s+", " "); // استبدال المسافات المتعددة بمسافة واحدة
                tipContent = tipContent.Trim();

                // تجاهل التوصيات القصيرة جداً أو الفارغة
                if (tipContent.Length < 20)
                    continue;

                // إضافة رقم التوصية في البداية
                var formattedTip = $"{tipNumber}. {tipContent}";

                tips.Add(new RecommendationTipDto
                {
                    Tip = formattedTip
                });
            }
        }

        // إذا لم نجد أي توصيات منظمة، نحاول استخراج الفقرات
        if (!tips.Any())
        {
            var paragraphs = text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                                .Where(p => !string.IsNullOrWhiteSpace(p) && p.Trim().Length > 30)
                                .Take(7);

            int counter = 1;
            foreach (var paragraph in paragraphs)
            {
                var cleanParagraph = paragraph.Trim().TrimStart('-', '•', '*', ' ');
                if (cleanParagraph.Length > 20)
                {
                    tips.Add(new RecommendationTipDto
                    {
                        Tip = $"{counter}. {cleanParagraph}"
                    });
                    counter++;
                }
            }
        }

        return tips;
    }
}

