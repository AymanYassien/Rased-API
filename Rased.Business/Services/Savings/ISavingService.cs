﻿using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Savings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.Savings
{
    public interface ISavingService
    {
            Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllSavingAsync();
            Task<ApiResponse<ReadSavingDto?>> GetSavingByIdAsync(int id);
            Task<ApiResponse<string>> AddSavingAsync(AddSavingDto addSavingDto);
            Task<ApiResponse<string>> UpdateSavingAsync(UpdateSavingDto updateSavingDto);
            Task<ApiResponse<string>> DeleteSavingAsync(int id);

        //Wallet
        Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllSavingsByWalletAsync(String userId, int walletId);
        Task<ApiResponse<decimal>> GetTotalSavingsByWalletAsync(string userId, int walletId);
        Task<ApiResponse<IQueryable<ReadSavingDto>>> GetAllTrueSavingsByWalletAsync(string userId, int walletId);





    }
}
