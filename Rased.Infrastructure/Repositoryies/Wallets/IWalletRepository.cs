using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;
using System.Linq.Expressions;

namespace Rased.Infrastructure.Repositoryies.Wallets
{
    public interface IWalletRepository : IRepository<Wallet, int>
    {
        // Here, You can add Methods other than the CRUD operations
        // ....
        // Get the status, color, and currency of a wallet with Id
        Task<WalletDataPartsDto> GetWalletDataPartsAsync(int id);
        // Check Method ..
        Task<StatusDto> CheckAsync(string userId, int colorId, int statusId, int currId, int walletId, string newWalletName, string? oldWalletName, bool isAdd);



    }
}
