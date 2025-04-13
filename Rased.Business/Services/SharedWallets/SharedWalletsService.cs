using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SharedWallets;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.SharedWallets
{
    public class SharedWalletsService: ISharedWalletsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SharedWalletsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create a new shared wallet
        public async Task<ApiResponse<string>> CreateAsync(SharedWalletDto model, string userId)
        {
            // Some Checks ...
            var check = await _unitOfWork.SharedWallets.CheckAsync(userId, model.ColorTypeId, model.WalletStatusId, model.CurrencyId, 0, model.Name, true);
            if (!check.IsSucceeded)
                return new ApiResponse<string>(check.Message!);

            var user = await _unitOfWork.SharedWallets.RasedUser(userId);
            var color = await _unitOfWork.SharedWallets.GetStaticColorTypeAsync(model.ColorTypeId);
            var status = await _unitOfWork.SharedWallets.GetStaticWalletStatusDataAsync(model.WalletStatusId);
            var currency = await _unitOfWork.SharedWallets.GetCurrencyAsync(model.CurrencyId);

            try
            {
                // [1] Add New Wallet
                var newWallet = new SharedWallet
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
                    CreatedAt = DateTime.Now,
                    StaticWalletStatusData = status,
                    StaticColorTypeData = color,
                    Creator = user,
                    Currency = currency
                };
                await _unitOfWork.SharedWallets.AddAsync(newWallet); // From Base
                // Save Changes
                await _unitOfWork.CommitChangesAsync();

                // [2] Get the access level Id
                var accessLevel = await _unitOfWork.SharedWallets.GetAccessLevelAsync(AccessLevelConstants.ADMIN);
                if (accessLevel is null)
                    return new ApiResponse<string>("خطأ تقني!");

                // [3] Add New Member to this Shared Wallet as an ADMIN
                var newMember = new SharedWalletMembers
                {
                    UserId = userId,
                    SharedWalletId = newWallet.SharedWalletId,
                    AccessLevelId = accessLevel.Id,
                    JoinedAt = DateTime.Now,
                    SharedWallet = newWallet,
                    Member = user,
                    StaticSharedWalletAccessLevelData = accessLevel
                };
                await _unitOfWork.SharedWallets.AddAsync<SharedWalletMembers>(newMember); // From Base
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "تم إضافة محفظة مشتركة جديدة بنجاح!");
        }

        // Update a shared wallet
        public async Task<ApiResponse<string>> UpdateAsync(SharedWalletDto model, int walletId, string userId)
        {
            // Some Checks ...
            var check = await _unitOfWork.SharedWallets.CheckAsync(userId, model.ColorTypeId, model.WalletStatusId, model.CurrencyId, walletId, model.Name, false);
            if (!check.IsSucceeded)
                return new ApiResponse<string>(check.Message!);

            // Get User, Color, Status, Currency
            var user = await _unitOfWork.SharedWallets.RasedUser(userId);
            var color = await _unitOfWork.SharedWallets.GetStaticColorTypeAsync(model.ColorTypeId);
            var status = await _unitOfWork.SharedWallets.GetStaticWalletStatusDataAsync(model.WalletStatusId);
            var currency = await _unitOfWork.SharedWallets.GetCurrencyAsync(model.CurrencyId);

            try
            {
                // Update Wallet
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == walletId && x.CreatorId == userId };
                var sw = _unitOfWork.SharedWallets.GetData(filters, null, true).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                sw.Name = model.Name;
                sw.Description = model.Description;
                sw.Icon = model.Icon;
                sw.InitialBalance = model.InitialBalance;
                sw.ExpenseLimit = model.ExpenseLimit;
                sw.CreatorId = userId;
                sw.CurrencyId = model.CurrencyId;
                sw.ColorTypeId = model.ColorTypeId;
                sw.WalletStatusId = model.WalletStatusId;
                sw.LastModified = DateTime.Now;
                sw.Creator = user;
                sw.Currency = currency;
                sw.StaticWalletStatusData = status;
                sw.StaticColorTypeData = color;
                // Update Wallet
                await _unitOfWork.SharedWallets.UpdateAsync(sw); // From Base
                // Save Changes
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "تم تحديث البيانات بنجاح!");
        }

        // Remove a specific shared wallet
        public async Task<ApiResponse<string>> RemoveAsync(int id, string userId)
        {
            try
            {
                // Filter Expression
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == id && x.CreatorId == userId };

                var sw = _unitOfWork.SharedWallets.GetData(filters, null, true).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                // Remove the wallet
                _unitOfWork.SharedWallets.Remove(sw);
                await _unitOfWork.CommitChangesAsync();
            }
            catch(Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null!, "تم حذف المحفظة المشتركة بنجاح!");
        }

        // Read a single shared wallet
        public async Task<ApiResponse<ReadSharedWalletDto>> ReadSingleAsync(int id, string userId)
        {
            var result = new ReadSharedWalletDto();

            try
            {
                // Filter Expression
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == id && x.CreatorId == userId };
                Expression<Func<SharedWallet, object>>[] includes = { x => x.Currency, x => x.StaticColorTypeData, x => x.StaticWalletStatusData };
                var sw = _unitOfWork.SharedWallets.GetData(filters, includes, false).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<ReadSharedWalletDto>("المحفظة المشتركة غير موجودة!");

                // Get the Access Level
                Expression<Func<SharedWalletMembers, bool>>[] filterMember = { x => x.SharedWalletId == id && x.UserId == userId };
                Expression<Func<SharedWalletMembers, object>>[] includeMember = { x => x.StaticSharedWalletAccessLevelData };
                var member = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filterMember, includeMember, false).FirstOrDefault();
                if (member == null)
                    return new ApiResponse<ReadSharedWalletDto>("خطأ تقني!");

                // Map Shared Wallet Data
                result = MapData(sw, member.StaticSharedWalletAccessLevelData.Name);
            }
            catch(Exception ex)
            {
                return new ApiResponse<ReadSharedWalletDto>(ex.Message);
            }
            
            return new ApiResponse<ReadSharedWalletDto>(result);
        }

        // Read All for the current user 
        public async Task<ApiResponse<IEnumerable<ReadSharedWalletDto>>> ReadAllAsync(string userId)
        {
            var result = new List<ReadSharedWalletDto>();

            try
            {
                // Filter Expression
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.CreatorId == userId };
                Expression<Func<SharedWallet, object>>[] includes = { x => x.Currency, x => x.StaticColorTypeData, x => x.StaticWalletStatusData };
                var sws = _unitOfWork.SharedWallets.GetData(filters, includes, false).AsEnumerable();
                // Check if there are no wallets
                if (!sws.Any())
                    return new ApiResponse<IEnumerable<ReadSharedWalletDto>>("لا يوجد محافظ مشتركة لديك!");

                // Mapping Wallets
                foreach (var sw in sws)
                {
                    // Get the Access Level
                    Expression<Func<SharedWalletMembers, bool>>[] filterMember = { x => x.SharedWalletId == sw.SharedWalletId && x.UserId == userId };
                    Expression<Func<SharedWalletMembers, object>>[] includeMember = { x => x.StaticSharedWalletAccessLevelData };
                    var member = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filterMember, includeMember, false).FirstOrDefault();
                    if (member == null)
                        return new ApiResponse<IEnumerable<ReadSharedWalletDto>>("خطأ تقني!");

                    // Map Shared Wallet Data and add it to the List
                    result.Add(MapData(sw, member.StaticSharedWalletAccessLevelData.Name));
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ReadSharedWalletDto>>(ex.Message);
            }


            return new ApiResponse<IEnumerable<ReadSharedWalletDto>>(result);
        }


        private static ReadSharedWalletDto MapData(SharedWallet sw, string accessLvl)
        {
            return new ReadSharedWalletDto
            {
                Id = sw.SharedWalletId,
                Name = sw.Name,
                Description = sw.Description,
                Icon = sw.Icon,
                InitialBalance = sw.InitialBalance,
                TotalBalance = sw.TotalBalance,
                ExpenseLimit = sw.ExpenseLimit,
                CreatedAt = sw.CreatedAt,
                UpdatedAt = sw.LastModified,
                Currency = sw.Currency.Name,
                Color = sw.StaticColorTypeData.Name,
                Status = sw.StaticWalletStatusData.Name,
                UserAccessLvl = accessLvl
            };
        }
    }
}
