using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.Wallets;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.Wallets
{
    public class WalletService: IWalletService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WalletService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<IEnumerable<ReadWalletDto>>> GetAllWalletsAsync(string curUserId)
        {
            var result = new List<ReadWalletDto>();

            Expression<Func<Wallet, bool>>[] filters = { x => x.CreatorId == curUserId };
            var wallets = _unitOfWork.Wallets.GetAll(filters, null, false).AsEnumerable();
            // Check if there are no wallets
            if (wallets.Count() <= 0)
                return new ApiResponse<IEnumerable<ReadWalletDto>>(null, "No Wallets Found");

            // Mapping Wallets
            foreach (var wallet in wallets)
            {
                // Get Wallet Data Parts
                var walletDataParts = await _unitOfWork.Wallets.GetWalletDataPartsAsync(wallet.WalletId);
                var walletData = MapWalletData(wallet, walletDataParts.Currency, walletDataParts.Color, walletDataParts.Status);
                result.Add(walletData);
            }

            return new ApiResponse<IEnumerable<ReadWalletDto>>(result);
        }

        public async Task<ApiResponse<string>> AddWalletAsync(RequestWalletDto model, string userId)
        {
            // Some Check ...
            var check = await _unitOfWork.Wallets.CheckAsync(userId, model.ColorTypeId, model.WalletStatusId, model.CurrencyId, 0, model.Name, null, true);
            if (!check.IsSucceeded)
                return new ApiResponse<string>(check.Message);
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
                    CreatedDate = DateTime.Now
                };
                await _unitOfWork.Wallets.AddAsync(newWallet); // From Base
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Wallet Added Successfully");
        }


        private ReadWalletDto MapWalletData(Wallet wallet, string curr, string color, string status)
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
