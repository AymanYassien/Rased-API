using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Wallets;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.Wallets
{
    public class WalletService : IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ReadWalletDto>>> GetAllWalletsAsync(string userId)
        {
            var result = new List<ReadWalletDto>();

            Expression<Func<Wallet, bool>>[] filters = { x => x.CreatorId == userId };
            var wallets = _unitOfWork.Wallets.GetData(filters, null, false).AsEnumerable();
            // Check if there are no wallets
            if (!wallets.Any())
                return new ApiResponse<IEnumerable<ReadWalletDto>>("No Wallets Found");

            // Mapping Wallets
            foreach (var wallet in wallets)
            {
                // Get Wallet Data Parts
                var walletDataParts = await _unitOfWork.Wallets.GetWalletDataPartsAsync(wallet.WalletId);
                var walletData = MapWalletData(wallet, walletDataParts.Currency ?? "", walletDataParts.Color ?? "", walletDataParts.Status ?? "");
                result.Add(walletData);
            }

            return new ApiResponse<IEnumerable<ReadWalletDto>>(result);
        }

        public async Task<ApiResponse<string>> AddWalletAsync(RequestWalletDto model, string userId)
        {
            // Some Checks ...
            var check = await _unitOfWork.Wallets.CheckAsync(userId, model.ColorTypeId, model.WalletStatusId, model.CurrencyId, 0, model.Name, true);
            if (!check.IsSucceeded)
                return new ApiResponse<string>(check.Message);

            var user = await _unitOfWork.Wallets.RasedUser(userId);
            var color = await _unitOfWork.Wallets.GetStaticColorTypeAsync(model.ColorTypeId);
            var status = await _unitOfWork.Wallets.GetStaticWalletStatusDataAsync(model.WalletStatusId);
            var currency = await _unitOfWork.Wallets.GetCurrencyAsync(model.CurrencyId);

            try
            {
                // Add New Wallet
                var newWallet = new Wallet
                {
                    Name = model.Name,
                    Description = model.Description,
                    Icon = model.Icon,
                    InitialBalance = model.InitialBalance,
                    TotalBalance = model.InitialBalance,
                    ExpenseLimit = model.ExpenseLimit,
                    CreatorId = userId,
                    CurrencyId = model.CurrencyId,
                    ColorTypeId = model.ColorTypeId,
                    WalletStatusId = model.WalletStatusId,
                    CreatedDate = DateTime.Now,
                    StaticWalletStatusData = status,
                    StaticColorTypeData = color,
                    Creator = user,
                    Currency = currency
                };
                await _unitOfWork.Wallets.AddAsync(newWallet); // From Base
                // Save Changes
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Wallet Added Successfully");
        }

        public async Task<ApiResponse<ReadWalletDto>> GetWalletByIdAsync(int id, string userId)
        {
            // Filter Expression
            Expression<Func<Wallet, bool>>[] filters = { x => x.WalletId == id && x.CreatorId == userId };

            var wallet = _unitOfWork.Wallets.GetData(filters, null, false).FirstOrDefault();
            if (wallet == null)
                return new ApiResponse<ReadWalletDto>("Wallet Not Found");
            // Get Wallet Data Parts
            var walletDataParts = await _unitOfWork.Wallets.GetWalletDataPartsAsync(wallet.WalletId);
            var walletData = MapWalletData(wallet, walletDataParts.Currency ?? "", walletDataParts.Color ?? "", walletDataParts.Status ?? "");

            return new ApiResponse<ReadWalletDto>(walletData);
        }

        public async Task<ApiResponse<string>> UpdateWalletAsync(RequestWalletDto model, int walletId, string userId)
        {
            // Some Checks ...
            var check = await _unitOfWork.Wallets.CheckAsync(userId, model.ColorTypeId, model.WalletStatusId, model.CurrencyId, walletId, model.Name, false);
            if (!check.IsSucceeded)
                return new ApiResponse<string>(check.Message!);

            // Get User, Color, Status, Currency
            var user = await _unitOfWork.Wallets.RasedUser(userId);
            var color = await _unitOfWork.Wallets.GetStaticColorTypeAsync(model.ColorTypeId);
            var status = await _unitOfWork.Wallets.GetStaticWalletStatusDataAsync(model.WalletStatusId);
            var currency = await _unitOfWork.Wallets.GetCurrencyAsync(model.CurrencyId);
            
            try
            {
                // Update Wallet
                Expression<Func<Wallet, bool>>[] filters = { x => x.WalletId == walletId && x.CreatorId == userId };
                var wallet = _unitOfWork.Wallets.GetData(filters, null, true).FirstOrDefault();
                if (wallet == null)
                    return new ApiResponse<string>("Wallet Not Found");

                decimal totalBalance = (model.InitialBalance - wallet.InitialBalance) + wallet.TotalBalance;

                wallet.Name = model.Name;
                wallet.Description = model.Description;
                wallet.Icon = model.Icon;
                wallet.InitialBalance = model.InitialBalance;
                wallet.TotalBalance = totalBalance;
                wallet.ExpenseLimit = model.ExpenseLimit;
                wallet.CreatorId = userId;
                wallet.CurrencyId = model.CurrencyId;
                wallet.ColorTypeId = model.ColorTypeId;
                wallet.WalletStatusId = model.WalletStatusId;
                wallet.LastModified = DateTime.Now;
                wallet.Creator = user;
                wallet.Currency = currency;
                wallet.StaticWalletStatusData = status;
                wallet.StaticColorTypeData = color;
                // Update Wallet
                await _unitOfWork.Wallets.UpdateAsync(wallet); // From Base
                // Save Changes
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Wallet Updated Successfully");
        }

        public async Task<ApiResponse<string>> RemoveWalletAsync(int id, string userId)
        {
            try
            {
                // Filter Expression
                Expression<Func<Wallet, bool>>[] filters = { x => x.WalletId == id && x.CreatorId == userId };

                var wallet = _unitOfWork.Wallets.GetData(filters, null, true).FirstOrDefault();
                if (wallet == null)
                    return new ApiResponse<string>("Wallet Not Found");

                // Remove the wallet
                _unitOfWork.Wallets.Remove(wallet);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null!, "Wallet Removed Successfully");
        }

        // Mapping Wallet Data
        private static ReadWalletDto MapWalletData(Wallet wallet, string curr, string color, string status)
        {
            return new ReadWalletDto
            {
                Id = wallet.WalletId,
                Name = wallet.Name,
                Description = wallet.Description,
                Icon = wallet.Icon,
                InitialBalance = wallet.InitialBalance,
                TotalBalance = wallet.TotalBalance,
                ExpenseLimit = wallet.ExpenseLimit,
                Currency = curr,
                Color = color,
                Status = status,
                CreatedAt = wallet.CreatedDate,
                UpdatedAt = wallet.LastModified
            };
        }
    }
}