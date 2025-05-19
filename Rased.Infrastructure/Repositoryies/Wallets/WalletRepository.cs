using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.Extras;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.Wallets
{
    public class WalletRepository : Repository<Wallet, int>, IWalletRepository
    {
        private readonly UserManager<RasedUser> _userManager;
        public WalletRepository(RasedDbContext context, UserManager<RasedUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        // Get All Wallets
        public async Task<WalletDataPartsDto> GetWalletDataPartsAsync(int id)
        {
            var result = new WalletDataPartsDto();

            var wallet = await _context.Wallets.Include(x => x.StaticWalletStatusData).Include(x => x.StaticColorTypeData).Include(x => x.Currency).FirstOrDefaultAsync(x => x.WalletId == id);
            if (wallet is null)
                return result;

            result.Status.Name = wallet.StaticWalletStatusData.Name;
            result.Color.Name = wallet.StaticColorTypeData.Name;
            result.Currency.Name = wallet.Currency.Name;

            result.Status.Id = wallet.StaticWalletStatusData.Id;
            result.Color.Id = wallet.StaticColorTypeData.Id;
            result.Currency.Id = wallet.Currency.Id;

            return result;
        }

        // Some Critical Checks
        public async Task<StatusDto> CheckAsync(string userId, int colorId, int statusId, int currId, int walletId, string walletName, bool isAdd)
        {
            var result = new StatusDto();

            // check if the user exists
            var user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                result.Message = "عذرًا، لا يمكنك الوصول!";
                return result;
            }

            // check if the wallet exists
            if (!isAdd)
            {
                var wallet = await _context.Wallets.FirstOrDefaultAsync(x => x.WalletId == walletId);
                if (wallet is null)
                {
                    result.Message = "المحفظة غير موجودة";
                    return result;
                }
                // Check if the Name exists
                var checkName = await _context.Wallets.FirstOrDefaultAsync(x => x.Name == walletName);
                if (checkName is not null && checkName.Name != walletName)
                {
                    result.Message = "اسم المحفظة مُكرر، حاول مرة أخري!";
                    return result;
                }
            }
            else
            {
                // check if the wallet name is unique
                var walletUnique = await _context.Wallets.FirstOrDefaultAsync(x => x.Name == walletName);
                if (walletUnique is not null)
                {
                    result.Message = "اسم المحفظة مُكرر، حاول مرة أخري!";
                    return result;
                }
            }

            // check if the color exists
            var color = await _context.StaticColorTypes.FirstOrDefaultAsync(x => x.Id == colorId);
            if (color is null)
            {
                result.Message = "من فضلك اختر لون محفظتك";
                return result;
            }

            // check if the status exists
            var status = await _context.StaticWalletStatus.FirstOrDefaultAsync(x => x.Id == statusId);
            if (status is null)
            {
                result.Message = "من فضلك اختر حالة المحفظة";
                return result;
            }

            // check if the currency exists
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == currId);
            if (currency is null)
            {
                result.Message = "من فضلك اختر عملة المحفظة";
                return result;
            }

            result.IsSucceeded = true;
            return result;
        }



        // Required Related Entities
        public async Task<RasedUser> RasedUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            return user!;
        }

        public async Task<StaticColorTypeData> GetStaticColorTypeAsync(int id)
        {
            var color = await _context.StaticColorTypes.FirstOrDefaultAsync(x => x.Id == id);
            return color!;
        }

        public async Task<StaticWalletStatusData> GetStaticWalletStatusDataAsync(int id)
        {
            var status = await _context.StaticWalletStatus.FirstOrDefaultAsync(x => x.Id == id);
            return status!;
        }

        public async Task<Currency> GetCurrencyAsync(int id)
        {
            var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Id == id);
            return currency!;
        }


        public async Task<bool> UpdateTotalBalance(int walletId, decimal amount)
        {
            var obj =  await GetByIdAsync(walletId);
            if (obj is not null )
            {
                obj.TotalBalance += amount;

                UpdateAsync(obj);
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }
       

    }
}
