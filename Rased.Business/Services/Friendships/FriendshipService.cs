using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Friendships;
using Rased.Business.Dtos.Response;
using Rased.Business.Services.AuthServices;
using Rased.Infrastructure;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.Friendships
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public FriendshipService(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        // Send Friend Request
        public async Task<ApiResponse<string>> SendFriendRequestAsync(SendFriendRequestDto model, string senderId)
        {
            try
            {
                // Check if the receiver exists
                var receiverId = await _unitOfWork.Friendships.GetUserIdByEmailAsync(model.ReceiverEmail);
                if (string.IsNullOrEmpty(receiverId))
                    return new ApiResponse<string>("هذا المستخدم غير موجود!");

                // Check if the user send a request to himself hahaha
                if(receiverId == senderId)
                    return new ApiResponse<string>("عملية خاطئة، حاول مرة أخري!");

                // Check if they are friends
                var filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == senderId && x.ReceiverId == receiverId && x.Status == InvitationStatusConstants.ACCEPTED };
                var checkFriend = await _unitOfWork.Friendships.GetData(filters, null, false).FirstOrDefaultAsync();
                if (checkFriend is not null)
                    return new ApiResponse<string>("أنتم بالفعل أصدقاء 🎉");

                // Check if the user send already a request (still pending)
                filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == senderId && x.ReceiverId == receiverId && x.Status == InvitationStatusConstants.PENDING };
                checkFriend = await _unitOfWork.Friendships.GetData(filters, null, false).FirstOrDefaultAsync();
                if (checkFriend is not null)
                    return new ApiResponse<string>("لقد أرسلت طلب صداقة لهذا المستخدم بالفعل!");

                // Check if the receiver user send a friend request which is already sent to him
                filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == receiverId && x.ReceiverId == senderId && x.Status == InvitationStatusConstants.PENDING };
                checkFriend = await _unitOfWork.Friendships.GetData(filters, null, false).FirstOrDefaultAsync();
                if (checkFriend is not null)
                    return new ApiResponse<string>("هذا المستخدم طلب منك الصداقة بالفعل، يمكنك مراجعته في قسم الأصدقاء!");

                // Get the sender data
                var sender = await _unitOfWork.Friendships.GetUserByIdAsync(senderId);

                // Add New Friendship record into Database
                var newFriendship = new Friendship
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Status = InvitationStatusConstants.PENDING,
                    SentAt = DateTime.Now
                };
                await _unitOfWork.Friendships.AddAsync(newFriendship);
                await _unitOfWork.CommitChangesAsync();

                // Send an Email to the receiver about a new friend request
                var senderName = $"{sender.FullName}";
                string emailSubject = $"📩 {senderName} أرسل لك طلب صداقة";
                string emailBody = $@"
                <h2 style='color: #2c3e50;'>👋 مرحبًا!</h2>
                <p>
                    لقد أرسل لك <strong>'{senderName}'</strong> طلب صداقة على التطبيق ✨
                </p>
                <p>
                    يمكنك الآن التعرف عليه ومشاركة اللحظات سويًا.
                </p>
                <p>
                    الرجاء فحص قسم الإشعارات في حسابك لمراجعة الطلب واتخاذ الإجراء المناسب.
                </p>
                <hr style='margin: 20px 0;'>
                <p style='color: #555; font-size: 14px;'>
                    مع أطيب التحيات،<br>
                    فريق <strong>راصِـــــد</strong> 💰
                </p>
                ";
                var sendEmail = await _emailService.SendEmailAsync(model.ReceiverEmail, emailSubject, emailBody);
                if (!sendEmail.successed)
                    return new ApiResponse<string>(null!, "تم إرسال طلب الصداقة بنجاح بنجاح .. خطأ تقني في إرسال إيميل بطلب الصداقة!");

                return new ApiResponse<string>(null!, "لقد تم إرسال طلب الصداقة بنجاح!");
            }
            catch(Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Update Friendship Status
        public async Task<ApiResponse<string>> UpdateFriendshipStatusAsync(UpdateFriendRequestDto model, string receiverId)
        {
            try
            {
                // Check if the friendship PENDING exists
                var filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == model.SenderId && x.ReceiverId == receiverId && x.Status == InvitationStatusConstants.PENDING };
                var friendship = await _unitOfWork.Friendships.GetData(filters, null, true).FirstOrDefaultAsync();
                if (friendship is null)
                    return new ApiResponse<string>("الصداقة غير موجودة!");

                if (model.Status) // Accepted Friendship
                {
                    // Update the Friendship Status
                    friendship.Status = InvitationStatusConstants.ACCEPTED;
                    friendship.UpdatedAt = DateTime.Now;
                    await _unitOfWork.Friendships.UpdateAsync(friendship);
                    await _unitOfWork.CommitChangesAsync();

                    // Get the sender and receiver data
                    var sender = await _unitOfWork.Friendships.GetUserByIdAsync(friendship.SenderId);
                    var receiver = await _unitOfWork.Friendships.GetUserByIdAsync(receiverId);
                    string receiverName = $"{receiver.FullName}";
                    string senderName = $"{sender.FullName}";
                    // Send an Email to the sender friend
                    string emailSubject = $"🎉 تم قبول طلب الصداقة الخاص بك!";
                    string emailBody = $@"
                    <h2 style='color: #2c3e50;'>🎉 تهانينا!</h2>

                    <p>
                        لقد قام <strong>'{receiverName}'</strong> بقبول طلب الصداقة الذي أرسلته بنجاح ✅
                    </p>

                    <p>
                        يمكنكم الآن التواصل ومشاركة اللحظات داخل التطبيق بسهولة.
                    </p>

                    <p>
                        قم بتسجيل الدخول إلى حسابك لبدء الدردشة أو الاطلاع على ملفه الشخصي.
                    </p>

                    <hr style='margin: 20px 0;'>

                    <p style='color: #555; font-size: 14px;'>
                        مع تحيات فريق <strong>راصِــــــد</strong> 💰
                    </p>
                    ";
                    var sendEmail = await _emailService.SendEmailAsync(sender.Email!, emailSubject, emailBody);
                    if (!sendEmail.successed)
                        return new ApiResponse<string>(null, $"تم إضافة '{senderName}' بنجاح إلي قائمة الأصدقاء" + " ... خطأ تقني في إرسال إيميل");

                    return new ApiResponse<string>(null, $"تم إضافة '{senderName}' بنجاح إلي قائمة الأصدقاء");
                }
                else // Canceled Friendship
                {
                    // Remove the Friendship
                    _unitOfWork.Friendships.Remove(friendship);
                    await _unitOfWork.CommitChangesAsync();

                    return new ApiResponse<string>(null, "تم إلغاء طلب الصداقة!");
                }
            }
            catch(Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Remove Friendship
        public async Task<ApiResponse<string>> RemoveFriendshipAsync(RemoveFriendshipDto model, string senderId)
        {
            try
            {
                // Check if the Friendship exists
                var filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == senderId && x.ReceiverId == model.ReceiverId && x.Status == InvitationStatusConstants.ACCEPTED };
                var friendship = await _unitOfWork.Friendships.GetData(filters, null, true).FirstOrDefaultAsync();
                if (friendship is null)
                    return new ApiResponse<string>("الصداقة غير موجودة!");

                // Delete the Friendship
                _unitOfWork.Friendships.Remove(friendship);
                await _unitOfWork.CommitChangesAsync();

                return new ApiResponse<string>(null, "تم إلغاء الصداقة!");
            }
            catch(Exception ex)
            {
                return new ApiResponse<string>(ex.Message);
            }
        }

        // Get All Current User Friends
        public async Task<ApiResponse<IEnumerable<UserFriendDto>>> GetUserFriendsAsync(string userId)
        {
            try
            {
                var userFriends = new List<UserFriendDto>();

                // Get the friends if the user is SENDER
                var filters = new Expression<Func<Friendship, bool>>[] { x => x.SenderId == userId };
                var includes = new Expression<Func<Friendship, object>>[] { x => x.Receiver };
                var friends = _unitOfWork.Friendships.GetData(filters, includes, false).AsEnumerable();
                if (friends.Any())
                {
                    foreach (var friend in friends)
                    {
                        var friendsDate = friend.UpdatedAt ?? DateTime.Now;
                        int friendsSince = (DateTime.Now - friendsDate).Days;

                        userFriends.Add(new UserFriendDto
                        {
                            FullName = $"{friend.Receiver.FullName}",
                            Email = friend.Receiver.Email,
                            ProfilePic = friend.Receiver.ProfilePic,
                            Country = friend.Receiver.Country,
                            FriendshipStatus = friend.Status,
                            FriendsSince = friendsSince
                        });
                    }
                }
                // To ensure No friends
                friends = null;
                // Get the friends if the user is RECEIVER
                filters = new Expression<Func<Friendship, bool>>[] { x => x.ReceiverId == userId };
                includes = new Expression<Func<Friendship, object>>[] { x => x.Sender };
                friends = _unitOfWork.Friendships.GetData(filters, includes, false).AsEnumerable();
                if (friends.Any())
                {
                    foreach (var friend in friends)
                    {
                        var friendsDate = friend.UpdatedAt ?? DateTime.Now;
                        int friendsSince = (DateTime.Now - friendsDate).Days;

                        userFriends.Add(new UserFriendDto
                        {
                            FullName = $"{friend.Sender.FullName}",
                            Email = friend.Sender.Email,
                            ProfilePic = friend.Sender.ProfilePic,
                            Country = friend.Sender.Country,
                            FriendshipStatus = friend.Status,
                            FriendsSince = friendsSince
                        });
                    }
                }

                if (!userFriends.Any())
                    return new ApiResponse<IEnumerable<UserFriendDto>>("لا يوجد لديك أصدقاء!");

                return new ApiResponse<IEnumerable<UserFriendDto>>(userFriends);
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<UserFriendDto>>(ex.Message);
            }
        }
    }
}
