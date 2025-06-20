﻿using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.SharedWallets
{
    public interface ISharedWalletRepository: IRepository<SharedWallet, int>
    {
        // Data Parts
        Task<WalletDataPartsDto> GetSharedWalletDataPartsAsync(int id);
        // Check Method ..
        Task<StatusDto> CheckAsync(string userId, int colorId, int statusId, int currId, int walletId, string walletName, bool isAdd);
        //Task<StaticSharedWalletAccessLevelData> GetAccessLevelAsync(string accessName);
        Task<string> GetUserIdByEmailAsync(string email);

        // Required Related Entities
        Task<RasedUser> RasedUser(string userId);
        Task<StaticColorTypeData> GetStaticColorTypeAsync(int id);
        Task<StaticWalletStatusData> GetStaticWalletStatusDataAsync(int id);
        Task<Currency> GetCurrencyAsync(int id);


        Task<bool> UpdateTotalBalance(int sharedWalletId, decimal amount);

        //Task<StatusDto> AddNewMember(RasedUser user, SharedWallet sw, StaticSharedWalletAccessLevelData accessLevel);

    }
}
