using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Recomm;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Recomm;
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

        public RecommendationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
    }



}
