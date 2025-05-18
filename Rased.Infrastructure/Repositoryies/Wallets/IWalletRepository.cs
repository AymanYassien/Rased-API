using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.Wallets
{
    public interface IWalletRepository : IRepository<Wallet, int>
    {
        // Here, You can add Methods other than the CRUD operations
        // ....
        // Get the status, color, and currency of a wallet with Id
        Task<WalletDataPartsDto> GetWalletDataPartsAsync(int id);
        // Check Method ..
        Task<StatusDto> CheckAsync(string userId, int colorId, int statusId, int currId, int walletId, string walletName, bool isAdd);
        //Task<StatusDto> AddNewWallet()

        // Required Related Entities
        Task<RasedUser> RasedUser(string userId);
        Task<StaticColorTypeData> GetStaticColorTypeAsync(int id);
        Task<StaticWalletStatusData> GetStaticWalletStatusDataAsync(int id);
        Task<Currency> GetCurrencyAsync(int id);

        Task<bool> UpdateTotalBalance(int walletId, decimal amount);
    }
}
