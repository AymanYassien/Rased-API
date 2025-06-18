using AutoMapper;
using AutoMapper.QueryableExtensions;
using MailKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Rased.Business.Dtos.Bills;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Recomm;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.RecommendSystem
{
    public class RecommendationService : IRecommendationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAiRecommendationService _aiService;
        private readonly ILogger<BudgetRecommendation> _logger;
        private readonly UserManager<RasedUser> _userManager;

        public RecommendationService(IUnitOfWork unitOfWork, IMapper mapper, IAiRecommendationService aiService,
        ILogger<BudgetRecommendation> logger,
        UserManager<RasedUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _aiService = aiService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new ApiResponse<IQueryable<BudgetRecommendationDto>>("User ID is required.");

            var recommendations = _unitOfWork.BudgetRecommendations
                .GetByCondition(r => r.UserId == userId)
                .OrderByDescending(r => r.GeneratedAt)
                .ProjectTo<BudgetRecommendationDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<BudgetRecommendationDto>>(recommendations);
        }

        public async Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByWalletAsync(int walletId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId);
            if (!walletExists)
                return new ApiResponse<IQueryable<BudgetRecommendationDto>>("Wallet not found.");

            var recommendations = _unitOfWork.BudgetRecommendations
                .GetByCondition(r => r.WalletId == walletId)
                .OrderByDescending(r => r.GeneratedAt)
                .ProjectTo<BudgetRecommendationDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<BudgetRecommendationDto>>(recommendations);
        }

        public async Task<ApiResponse<IQueryable<BudgetRecommendationDto>>> GetRecommendationsByWalletGroupAsync(int walletGroupId)
        {
            bool groupExists = await _unitOfWork.SharedWallets.AnyAsync(wg => wg.SharedWalletId == walletGroupId);
            if (!groupExists)
                return new ApiResponse<IQueryable<BudgetRecommendationDto>>("Wallet group not found.");

            var recommendations = _unitOfWork.BudgetRecommendations
                .GetByCondition(r => r.WalletGroupId == walletGroupId)
                .OrderByDescending(r => r.GeneratedAt)
                .ProjectTo<BudgetRecommendationDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<BudgetRecommendationDto>>(recommendations);
        }

        public async Task<ApiResponse<string>> MarkRecommendationAsReadAsync(int recommendationId)
        {
            var recommendation = await _unitOfWork.BudgetRecommendations.GetByIdAsync(recommendationId);
            if (recommendation == null)
                return new ApiResponse<string>("Recommendation not found.");

            recommendation.IsRead = true;
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Recommendation marked as read.");
        }

        public async Task<ApiResponse<string>> CreateRecommendationAsync(CreateBudgetRecommendationDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return new ApiResponse<string>("User ID is required.");

            if (dto.WalletId == null && dto.WalletGroupId == null)
                return new ApiResponse<string>("WalletId or WalletGroupId must be provided.");

            var recommendation = new BudgetRecommendation
            {
                Title = dto.Title,
                Description = dto.Description,
                UserId = dto.UserId,
                WalletId = dto.WalletId,
                WalletGroupId = dto.WalletGroupId,
                GeneratedAt = DateTime.UtcNow
            };

            await _unitOfWork.BudgetRecommendations.AddAsync(recommendation);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Recommendation created successfully.");
        }


        public async Task GenerateMonthlyRecommendationsAsync()
        {
            var currentMonth = DateTime.Now.Month;
            var currentYear = DateTime.Now.Year;

            _logger.LogInformation($"Starting monthly recommendations generation for {currentMonth}/{currentYear}");

            try
            {
                var activeWallets = _unitOfWork.Wallets.GetAll();
                var processedCount = 0;
                var errorCount = 0;

                foreach (var wallet in activeWallets)
                {
                    try
                    {
                        // التحقق من عدم وجود توصية لهذا الشهر
                        var hasRecommendation = await HasRecommendationForMonthAsync(wallet.CreatorId, wallet.WalletId, currentMonth, currentYear);

                        if (hasRecommendation)
                        {
                            _logger.LogInformation($"Recommendation already exists for wallet {wallet.WalletId} in {currentMonth}/{currentYear}");
                            continue;
                        }

                        // التحقق من وجود بيانات كافية (آخر 30 يوم)
                        var hasData = await HasSufficientDataAsync(wallet.WalletId);
                        if (!hasData)
                        {
                            _logger.LogInformation($"Insufficient data for wallet {wallet.WalletId}");
                            continue;
                        }

                        // إنشاء التوصيات
                        var recommendations = await _aiService.GetRecommendationAsync(wallet.WalletId, wallet.CreatorId);

                        if (recommendations?.Any() == true)
                        {
                            foreach (var tip in recommendations.Take(5))
                            {
                                var monthlyRec = new BudgetRecommendation
                                {
                                    UserId = wallet.CreatorId,
                                    WalletId = wallet.WalletId,
                                    Title = $"توصية مالية - {DateTime.Now:MMMM yyyy}",
                                    Description = tip.Tip,
                                    GeneratedAt = DateTime.Now,
                                    IsRead = false,
                                    Month = currentMonth,
                                    Year = currentYear
                                };

                                await _unitOfWork.BudgetRecommendations.AddAsync(monthlyRec);
                            }

                            await _unitOfWork.CommitChangesAsync();
                            processedCount++;

                            _logger.LogInformation($"Generated recommendations for wallet {wallet.WalletId}");
                        }

                        await Task.Delay(2000); // تأخير بسيط لتفادي Rate Limiting
                    }
                    catch (Exception ex)
                    {
                        errorCount++;
                        _logger.LogError(ex, $"Error generating recommendations for wallet {wallet.WalletId}");
                    }
                }

                _logger.LogInformation($"Monthly recommendations generation completed. Processed: {processedCount}, Errors: {errorCount}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in monthly recommendations generation process");
            }
        }

        private async Task<bool> HasSufficientDataAsync(int walletId)
        {
            var thirtyDaysAgo = DateTime.Now.AddDays(-30);

            var recentTransactionsCount = await _unitOfWork.Expenses
                .CountAsync(e => e.WalletId == walletId && e.Date >= thirtyDaysAgo);

            var recentIncomesCount = await _unitOfWork.Income
                .CountAsync(i => i.WalletId == walletId && i.CreatedDate >= thirtyDaysAgo);

            return (recentTransactionsCount + recentIncomesCount) >= 3;
        }

        private async Task<bool> HasRecommendationForMonthAsync(string userId, int walletId, int month, int year)
        {
            return await _unitOfWork.BudgetRecommendations
                .AnyAsync(r => r.UserId == userId && r.WalletId == walletId && r.Month == month && r.Year == year);
        }

    }
}
