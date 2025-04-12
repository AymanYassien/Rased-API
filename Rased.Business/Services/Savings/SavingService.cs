using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using Rased.Infrastructure.Models.Savings;
using Rased.Infrastructure.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Savings
{
    public class SavingService : ISavingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllSavingAsync()
        {
            var savings = _unitOfWork.SavingRepository.GetAll()
                .ProjectTo<ReadSavingDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadSavingDto>>(savings);
        }

        public async Task<ApiResponse<ReadSavingDto>> GetSavingByIdAsync(int id)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(id);
            if (saving == null)
                return new ApiResponse<ReadSavingDto>("Saving not found.");

            return new ApiResponse<ReadSavingDto>(_mapper.Map<ReadSavingDto>(saving));
        }

        public async Task<ApiResponse<string>> AddSavingAsync(AddSavingDto addSavingDto)
        {
            await _unitOfWork.SavingRepository.AddAsync(_mapper.Map<Saving>(addSavingDto));
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving added successfully.");
        }

        public async Task<ApiResponse<string>> UpdateSavingAsync(UpdateSavingDto updateSavingDto)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(updateSavingDto.Id);
            if (saving == null)
                return new ApiResponse<string>("Saving not found.");

            _mapper.Map(updateSavingDto, saving);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving Updated successfully.");
        }

        public async Task<ApiResponse<string>> DeleteSavingAsync(int id)
        {
            var saving = await _unitOfWork.SavingRepository.GetByIdAsync(id);
            if (saving == null)
                return new ApiResponse<string>("Saving not found.");

            await _unitOfWork.SavingRepository.DeleteByIdAsync(id);
            await _unitOfWork.CommitChangesAsync();

            return new ApiResponse<string>(null, "Saving Deleted successfully.");
        }

        // Wallet
        public async Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllSavingsByWalletAsync(String userId, int walletId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);


            if (!walletExists)
                return new ApiResponse<IQueryable<ReadSavingDto>>("Wallet not found or doesn't belong to the user.");

            var savings = _unitOfWork.SavingRepository
                .FindAll(s => s.WalletId == walletId)
                .ProjectTo<ReadSavingDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadSavingDto>>(savings);
        }
        public async Task<ApiResponse<decimal>> GetTotalSavingsByWalletAsync(string userId, int walletId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);


            if (!walletExists)
                return new ApiResponse<Decimal>("Wallet not found or doesn't belong to the user.");

            var totalSavings = await _unitOfWork.SavingRepository
                .FindAll(s => s.WalletId == walletId)
                .SumAsync(s => s.TotalAmount);

            return new ApiResponse<decimal>(totalSavings);
        }
        public async Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllTrueSavingsByWalletAsync(string userId, int walletId)
        {
            bool walletExists = await _unitOfWork.Wallets.AnyAsync(w => w.WalletId == walletId && w.CreatorId == userId);


            if (!walletExists)
                return new ApiResponse<IQueryable<ReadSavingDto>>("Wallet not found or doesn't belong to the user.");

           
            var savings = _unitOfWork.SavingRepository
                .FindAll(s => s.WalletId == walletId && s.IsSaving)
                .ProjectTo<ReadSavingDto>(_mapper.ConfigurationProvider);

            return new ApiResponse<IQueryable<ReadSavingDto>>(savings);
        }






    }

}
