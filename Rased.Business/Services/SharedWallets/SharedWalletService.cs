using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SharedWallets;
using Rased.Business.Services.AuthServices;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.SharedWallets
{
    public class SharedWalletService: ISharedWalletService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public SharedWalletService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
                // [1] Add New Shared Wallet
                var newSW = new SharedWallet
                {
                    Name = model.Name,
                    Description = model.Description,
                    Icon = model.Icon,
                    InitialBalance = model.InitialBalance,
                    TotalBalance = model.InitialBalance,
                    ExpenseLimit = model.ExpenseLimit,
                    CurrencyId = model.CurrencyId,
                    ColorTypeId = model.ColorTypeId,
                    WalletStatusId = model.WalletStatusId,
                    CreatedAt = DateTime.Now,
                    StaticWalletStatusData = status,
                    StaticColorTypeData = color,
                    Currency = currency
                };
                await _unitOfWork.SharedWallets.AddAsync(newSW); // From Base
                await _unitOfWork.CommitChangesAsync();

                // [2] Add a new Member as OWNER
                var newMember = new SharedWalletMembers
                {
                    UserId = userId,
                    SharedWalletId = newSW.SharedWalletId,
                    Role = AccessLevelConstants.OWNER,
                    JoinedAt = DateTime.Now
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

            // Ensure that the UserId is the OWNER
            Expression<Func<SharedWalletMembers, bool>>[] ownerFilter = { x => x.SharedWalletId == walletId && x.UserId == userId };
            var ownerCheck = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(ownerFilter, null, false).FirstOrDefault();
            if(ownerCheck is null || ownerCheck.Role != AccessLevelConstants.OWNER)
                return new ApiResponse<string>("لا يمكنك الوصول!");

            // Get User, Color, Status, Currency
            var user = await _unitOfWork.SharedWallets.RasedUser(userId);
            var color = await _unitOfWork.SharedWallets.GetStaticColorTypeAsync(model.ColorTypeId);
            var status = await _unitOfWork.SharedWallets.GetStaticWalletStatusDataAsync(model.WalletStatusId);
            var currency = await _unitOfWork.SharedWallets.GetCurrencyAsync(model.CurrencyId);

            try
            {
                // Update Wallet
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == walletId };
                var sw = _unitOfWork.SharedWallets.GetData(filters, null, true).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                decimal totalBalance = (model.InitialBalance - sw.InitialBalance) + sw.TotalBalance;

                sw.Name = model.Name;
                sw.Description = model.Description;
                sw.Icon = model.Icon;
                sw.InitialBalance = model.InitialBalance;
                sw.TotalBalance = totalBalance;
                sw.ExpenseLimit = model.ExpenseLimit;
                sw.CurrencyId = model.CurrencyId;
                sw.ColorTypeId = model.ColorTypeId;
                sw.WalletStatusId = model.WalletStatusId;
                sw.LastModified = DateTime.Now;
                sw.Currency = currency;
                sw.StaticWalletStatusData = status;
                sw.StaticColorTypeData = color;
                // Update Wallet
                await _unitOfWork.SharedWallets.UpdateAsync(sw); // From Base
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
                // Ensure that the UserId is the OWNER
                Expression<Func<SharedWalletMembers, bool>>[] ownerFilter = { x => x.SharedWalletId == id && x.UserId == userId };
                var ownerCheck = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(ownerFilter, null, false).FirstOrDefault();
                if (ownerCheck is null || ownerCheck.Role != AccessLevelConstants.OWNER)
                    return new ApiResponse<string>("لا يمكنك الوصول!");

                // Filter Expression
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == id };
                var sw = _unitOfWork.SharedWallets.GetData(filters, null, true).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                // Remove the wallet
                _unitOfWork.SharedWallets.Remove(sw);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null!, "تم حذف المحفظة المشتركة بنجاح!");
        }

        // Read a single shared wallet
        public ApiResponse<ReadSharedWalletDto> ReadSingleAsync(int id, string userId)
        {
            var result = new ReadSharedWalletDto();

            try
            {
                // Get the shared wallet
                Expression<Func<SharedWallet, bool>>[] filters = { x => x.SharedWalletId == id };
                Expression<Func<SharedWallet, object>>[] includes = { x => x.Currency, x => x.StaticColorTypeData, x => x.StaticWalletStatusData };
                var sw = _unitOfWork.SharedWallets.GetData(filters, includes, false).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<ReadSharedWalletDto>("المحفظة المشتركة غير موجودة!");

                // Ensure that the UserId is a MEMBER
                Expression<Func<SharedWalletMembers, bool>>[] memberFilter = { x => x.SharedWalletId == id && x.UserId == userId };
                var memberCheck = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(memberFilter, null, false).FirstOrDefault();
                if (memberCheck is null)
                    return new ApiResponse<ReadSharedWalletDto>("لا يمكنك الوصول!");

                // Get the Members of this shared wallet
                Expression<Func<SharedWalletMembers, bool>>[] filterMember = { x => x.SharedWalletId == id };
                Expression<Func<SharedWalletMembers, object>>[] includeMember = { x => x.Member };
                var members = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filterMember, includeMember, false).AsEnumerable();
                if (!members.Any())
                    return new ApiResponse<ReadSharedWalletDto>("خطأ تقني!");

                // Map Shared Wallet Data
                result = MapData(sw, members);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ReadSharedWalletDto>(ex.Message);
            }

            return new ApiResponse<ReadSharedWalletDto>(result);
        }

        // Read all shared wallets which a specific user is a member in it
        public ApiResponse<IEnumerable<ReadSharedWalletDto>> ReadAllAsync(string userId)
        {
            var result = new List<ReadSharedWalletDto>();

            try
            {
                // Get the shared wallets from Members table
                Expression<Func<SharedWalletMembers, bool>>[] filters = { x => x.UserId == userId };
                Expression<Func<SharedWalletMembers, object>>[] includes = { x => x.Member, x => x.SharedWallet, x => x.SharedWallet.StaticColorTypeData, x => x.SharedWallet.StaticWalletStatusData, x => x.SharedWallet.Currency };
                var memberWallets = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filters, includes, false).AsEnumerable();
                // Check if there are no wallets
                if (!memberWallets.Any())
                    return new ApiResponse<IEnumerable<ReadSharedWalletDto>>("لا يوجد محافظ مشتركة لديك!");

                // Mapping Wallets
                foreach (var sw in memberWallets)
                {
                    // Get all Members of this shared wallet
                    Expression<Func<SharedWalletMembers, bool>>[] filterMember = { x => x.SharedWalletId == sw.SharedWalletId };
                    Expression<Func<SharedWalletMembers, object>>[] includeMember = { x => x.Member };
                    var members = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filterMember, includeMember, false).AsEnumerable();

                    // Map Shared Wallet Data and add it to the List
                    result.Add(MapData(sw.SharedWallet, members));
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<ReadSharedWalletDto>>(ex.Message);
            }

            return new ApiResponse<IEnumerable<ReadSharedWalletDto>>(result);
        }

        // Send an invitation (All participants can send invitations)
        public async Task<ApiResponse<string>> SendInviteAsync(SWInvitationDto model, string senderId)
        {
            try
            {
                // Check if the shared wallet exists
                var swFilter = new Expression<Func<SharedWallet, bool>>[] { x => x.SharedWalletId == model.SWId };
                var sw = _unitOfWork.SharedWallets.GetData(swFilter, null, false).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                // Check if the receiver email exists
                var receiverId = await _unitOfWork.SharedWallets.GetUserIdByEmailAsync(model.ReceiverEmail);
                if (string.IsNullOrEmpty(receiverId))
                    return new ApiResponse<string>("هذا المستخدم غير موجود!");

                // Check if the sender is a member in this wallet
                Expression<Func<SharedWalletMembers, bool>>[] filters = { x => x.UserId == senderId && x.SharedWalletId == model.SWId };
                var includes = new Expression<Func<SharedWalletMembers, object>>[] { x => x.Member };
                var sender = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filters, includes, false).FirstOrDefault();
                if (sender == null)
                    return new ApiResponse<string>("خطأ ما حدث!");

                // Check if the receiver is already a member in this wallet
                filters = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.UserId == receiverId && x.SharedWalletId == model.SWId };
                var receiverMember = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filters, null, false).FirstOrDefault();
                if (receiverMember != null)
                    return new ApiResponse<string>("هذا المستخدم هو بالفعل عضو في هذه المحفظة المشتركة!");

                // Check if the receiver is already invited
                var inviteFilter = new Expression<Func<SWInvitation, bool>>[] { x => x.ReceiverId == receiverId && x.SharedWalletId == model.SWId && x.Status == InvitationStatusConstants.PENDING };
                var invite = _unitOfWork.SharedWallets.GetData<SWInvitation>(inviteFilter, null, false).FirstOrDefault();
                if (invite != null)
                    return new ApiResponse<string>("تم إرسال دعوة سابقة لهذا المستخدم بالفعل!");

                // Now we can send the invitation
                var newInvite = new SWInvitation
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    SharedWalletId = model.SWId,
                    Status = InvitationStatusConstants.PENDING,
                    InvitedAt = DateTime.Now
                };
                // Add New invitation to database
                await _unitOfWork.SharedWallets.AddAsync<SWInvitation>(newInvite); // From Base
                await _unitOfWork.CommitChangesAsync();

                // Get Some Data to the email service
                filters = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.SharedWalletId == model.SWId };
                includes = new Expression<Func<SharedWalletMembers, object>>[] { x => x.Member };
                var swMembers = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(filters, includes, false).AsEnumerable();
                var ownerMember = swMembers.FirstOrDefault(x => x.Role == AccessLevelConstants.OWNER);
                string ownerName = ownerMember == null ? "UnKnown" : $"{ownerMember!.Member.FirstName} {ownerMember.Member.LastName}";
                var senderName = $"{sender.Member.FirstName} {sender.Member.LastName}";
                // Send an Email
                string emailSubject = $"📩 دعوة للانضمام إلى المحفظة المشتركة: {sw.Name}";
                string emailBody = $@"
                <p>مرحبًا،</p>

                <p>
                    قام <strong>{senderName}</strong> بدعوتك للانضمام إلى المحفظة المشتركة 
                    <strong>({sw.Name})</strong>.
                </p>

                <p style='margin-top: 15px;'>📌 <strong>تفاصيل المحفظة:</strong></p>
                <ul style='list-style: none; padding: 0;'>
                    <li>💰 <strong>الميزانية:</strong> {sw.TotalBalance}</li>
                    <li>👤 <strong>المالك:</strong> {ownerName}</li>
                    <li>👥 <strong>عدد الأعضاء:</strong> {swMembers.Count()}</li>
                    <li>📅 <strong>تاريخ الإنشاء:</strong> {sw.CreatedAt:d}</li>
                </ul>

                <p>
                    إذا كنت مهتمًا بالانضمام، يُرجى التحقق من قسم <strong>الإشعارات</strong> في حسابك والموافقة على الدعوة.
                </p>

                <p style='margin-top: 25px; color: #555; font-size: 14px;'>
                    مع تحيات فريق <strong>راصِــــــد</strong> 💰
                </p>
                ";
                var sendEmail = await _emailService.SendEmailAsync(model.ReceiverEmail, emailSubject, emailBody);
                if(!sendEmail.successed)
                    return new ApiResponse<string>(null, "تم إرسال الدعوة بنجاح .. خطأ تقني في إرسال إيميل بالدعوة!");

                return new ApiResponse<string>(null, "تم إرسال الدعوة بنجاح!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Update the invitation status
        public async Task<ApiResponse<string>> UpdateInviteStatusAsync(UpdateInvitationDto model, string receiverId)
        {
            try
            {
                // Check if the the invitation exists
                var inviteFilter = new Expression<Func<SWInvitation, bool>>[] { x => x.ReceiverId == receiverId && x.SharedWalletId == model.SWId && x.Status == InvitationStatusConstants.PENDING };
                var includes = new Expression<Func<SWInvitation, object>>[] { x => x.Receiver, x => x.SharedWallet };
                var invite = _unitOfWork.SharedWallets.GetData<SWInvitation>(inviteFilter, includes, true).FirstOrDefault();
                if (invite == null)
                    return new ApiResponse<string>("الدعوة غير موجودة!");

                // If Accepted
                if (model.Status)
                {
                    // Update Invitation Data
                    invite.Status = InvitationStatusConstants.ACCEPTED;
                    await _unitOfWork.SharedWallets.UpdateAsync<SWInvitation>(invite); // From Base
                    // Insert New Member
                    var newMember = new SharedWalletMembers
                    {
                        UserId = receiverId,
                        SharedWalletId = model.SWId,
                        Role = AccessLevelConstants.PARTICIPANT,
                        JoinedAt = DateTime.Now
                    };
                    await _unitOfWork.SharedWallets.AddAsync<SharedWalletMembers>(newMember); // From Base
                    // Save Changes
                    await _unitOfWork.CommitChangesAsync();

                    // Get the OWNER of the shared wallet and Send Email to him
                    var ownerFilter = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.SharedWalletId == model.SWId && x.Role == AccessLevelConstants.OWNER };
                    var ownerIncludes = new Expression<Func<SharedWalletMembers, object>>[] { x => x.Member };
                    var ownerMember = await _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(ownerFilter, ownerIncludes, false).FirstOrDefaultAsync();
                    if (ownerMember is not null)
                    {
                        string memberName = $"{invite.Receiver.FirstName} {invite.Receiver.LastName}";
                        string memberEmail = invite.Receiver.Email!;
                        string swName = invite.SharedWallet.Name;
                        string emailSubject = $"عضو جديد في المحفظة المشتركة ’{swName}’ ✅";
                        string emailBody = $@"
                        <p>مرحبًا،</p>

                        <p>
                            قام المستخدم <strong>{memberName}</strong> بقبول الدعوة والانضمام إلى المحفظة المشتركة 
                            <strong>({swName})</strong>.
                        </p>

                        <p style='margin-top: 15px;'>🎉 <strong>تفاصيل العضو الجديد:</strong></p>
                        <ul style='list-style: none; padding: 0;'>
                            <li>👤 <strong>الاسم:</strong> {memberName}</li>
                            <li>📧 <strong>البريد الإلكتروني:</strong> {memberEmail}</li>
                            <li>📅 <strong>تاريخ القبول:</strong> {newMember.JoinedAt:d}</li>
                        </ul>

                        <p>
                            يمكنك الآن إدارة إعدادات المحفظة ومشاركة الصلاحيات مع الأعضاء الجدد حسب الحاجة.
                        </p>

                        <p style='margin-top: 25px; color: #555; font-size: 14px;'>
                            مع تحيات فريق <strong>راصِــــــد</strong> 💰
                        </p>
                        ";

                        var sendEmail = await _emailService.SendEmailAsync(ownerMember.Member.Email!, emailSubject, emailBody);
                        if(!sendEmail.successed)
                            return new ApiResponse<string>(null, "تم قبول الدعوة بنجاح .. خطأ في إرسال إيميل للمالك");
                    }

                    return new ApiResponse<string>(null, "تم قبول الدعوة بنجاح!");
                }
                else // If Canceled
                {
                    // Delete the invite record
                    _unitOfWork.SharedWallets.Remove<SWInvitation>(invite); // From Base
                    await _unitOfWork.CommitChangesAsync();

                    return new ApiResponse<string>(null, "تم إلغاء الدعوة بنجاح!");
                }
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Update Member Access Level
        public async Task<ApiResponse<string>> UpdateMemberAccessLevelAsync(UpdateMemberRoleDto model, string userId)
        {
            try
            {
                // Check if the shared wallet exists
                var swFilter = new Expression<Func<SharedWallet, bool>>[] { x => x.SharedWalletId == model.SWId };
                var sw = _unitOfWork.SharedWallets.GetData(swFilter, null, false).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                // Check if the user is the owner of this wallet
                var ownerFilter = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.UserId == userId && x.SharedWalletId == model.SWId };
                var ownerMember = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(ownerFilter, null, false).FirstOrDefault();
                if (ownerMember == null || ownerMember.Role != AccessLevelConstants.OWNER)
                    return new ApiResponse<string>("لا يمكنك الوصول!");

                // Check if the member exists
                var memberFilter = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.UserId == model.MemberId && x.SharedWalletId == model.SWId };
                var member = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(memberFilter, null, true).FirstOrDefault();
                if (member == null)
                    return new ApiResponse<string>("هذا المستخدم ليس عضوًا في هذه المحفظة المشتركة!");
                // The Old Role
                string oldRole = member.Role;

                // Update Member Access Level
                member.Role = model.NewRole;
                await _unitOfWork.SharedWallets.UpdateAsync(member); // From Base
                await _unitOfWork.CommitChangesAsync();

                return new ApiResponse<string>(null!, $"تم تحديث إمكانية وصول المستخدم من {oldRole} إلي {model.NewRole} لهذه المحفظة المشتركة");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Remove Member from shared wallet
        public async Task<ApiResponse<string>> RemoveMemberAsync(RemoveMemberDto model, string userId)
        {
            try
            {
                // Ensure that the userId is either Owner or SuperVisor
                var ownerFilter = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.UserId == userId && x.SharedWalletId == model.SWId };
                var ownerMember = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(ownerFilter, null, false).FirstOrDefault();
                if (ownerMember == null || (ownerMember.Role != AccessLevelConstants.OWNER && ownerMember.Role != AccessLevelConstants.SUPERVISOR))
                    return new ApiResponse<string>("لا يمكنك الوصول!");

                // Check if the userId == receiverId
                if (userId == model.MemberId)
                    return new ApiResponse<string>("خطأ ما حدث، لا يمكنك حذف نفسك 😂");

                // Check if the shared wallet exists
                var swFilter = new Expression<Func<SharedWallet, bool>>[] { x => x.SharedWalletId == model.SWId };
                var sw = _unitOfWork.SharedWallets.GetData(swFilter, null, false).FirstOrDefault();
                if (sw == null)
                    return new ApiResponse<string>("المحفظة المشتركة غير موجودة!");

                // Check if the MemberId is the Owner
                var memberFilter = new Expression<Func<SharedWalletMembers, bool>>[] { x => x.UserId == model.MemberId && x.SharedWalletId == model.SWId };
                var member = _unitOfWork.SharedWallets.GetData<SharedWalletMembers>(memberFilter, null, true).FirstOrDefault();
                if (member == null)
                    return new ApiResponse<string>("هذا المستخدم ليس عضوًا في هذه المحفظة المشتركة!");
                if (member.Role == AccessLevelConstants.OWNER)
                    return new ApiResponse<string>("خطأ، عملية غير صحيحة!");

                // Remove The Member
                _unitOfWork.SharedWallets.Remove<SharedWalletMembers>(member);
                await _unitOfWork.CommitChangesAsync();

                return new ApiResponse<string>(null, "تم حذف العضو بنجاح");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }


        // Get The Shared Wallet Data
        private static ReadSharedWalletDto MapData(SharedWallet sw, IEnumerable<SharedWalletMembers>? members)
        {
            // Get the Members
            var membersList = members?.Select(m => new SWMembersDto
            {
                Email = m.Member.Email,
                FullName = $"{m.Member.FirstName} {m.Member.LastName}",
                Role = m.Role
            }).ToList();

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
                Members = membersList
            };
        }
    }
}
