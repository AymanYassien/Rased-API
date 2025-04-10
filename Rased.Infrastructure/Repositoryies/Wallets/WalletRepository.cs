using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;
using System.Linq.Expressions;

namespace Rased.Infrastructure.Repositoryies.Wallets
{
    public class WalletRepository : Repository<Wallet, int>, IWalletRepository
    {
        private readonly UserManager<RasedUser> _userManager;
        public WalletRepository(RasedDbContext context, UserManager<RasedUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        // Extra Methods ...
        public async Task<WalletDataPartsDto> GetWalletDataPartsAsync(int id)
        {
            var result = new WalletDataPartsDto();

            var wallet = await _context.Wallets.Include(x => x.StaticWalletStatusData).Include(x => x.StaticColorTypeData).Include(x => x.Currency).FirstOrDefaultAsync(x => x.WalletId == id);
            if (wallet is null)
                return result;

            result.Status = wallet.StaticWalletStatusData.Name;
            result.Color = wallet.StaticColorTypeData.Name;
            result.Currency = wallet.Currency.Name;

            return result;
        }

        public async Task<StatusDto> CheckAsync(string userId, int colorId, int statusId, int currId, int walletId, string newWalletName, string? oldWalletName, bool isAdd)
        {
            var result = new StatusDto();

            // Handle Update Checks ....

            // check if the wallet exists
            if (!isAdd)
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletId == walletId);
                if (wallet is null)
                {
                    result.Message = "Wallet is Not Found";
                    return result;
                }
            }
            // check if the color exists
            var color = await _context.StaticColorTypes.FirstOrDefaultAsync(x => x.id == colorId);
            if (color is null)
            {
                result.Message = "Color is Not Found";
                return result;
            }
            // check if the status exists
            var status = await _context.StaticWalletStatus.FirstOrDefaultAsync(x => x.id == statusId);
            if (status is null)
            {
                result.Message = "Status is Not Found";
                return result;
            }
            // check if the currency exists
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == currId);
            if (currency is null)
            {
                result.Message = "Currency is Not Found";
                return result;
            }
            // check if the user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                result.Message = "Oops, Can't Access!!";
                return result;
            }
            // check if the wallet name is unique
            var walletUnique = await _context.Wallets.FirstOrDefaultAsync(x => x.Name == newWalletName);
            if(walletUnique is not null)
            {
                result.Message = "Wallet Name should be Unique!";
                return result;
            }


            result.IsSucceeded = true;
            return result;
        }

    

       
    }
}
