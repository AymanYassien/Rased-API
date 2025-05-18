using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rased.Business.Dtos;
using Rased.Business.Dtos.Auths;
using Rased.Business.Dtos.Response;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.User;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rased.Business.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<RasedUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly RoleManager<CustomRole> _roleManager;

        public AuthService( UserManager<RasedUser> userManager , IEmailService emailService , IConfiguration configuration
            ,RoleManager<CustomRole> roleManager)
        {
            _userManager = userManager;
            _emailService = emailService;
            _configuration = configuration;
            _roleManager = roleManager;
        }

        // Register
        public async Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                // Some Checks
                if (_userManager.Users.Any(s => s.Email == registerDto.Email))
                {
                    return new ApiResponse<string>("هذا الإيميل يمتلكه شخص آخر، جرب واحد آخر");
                }
                if (registerDto.Password != registerDto.ConfirmedPassword)
                {
                    return new ApiResponse<string>("لابد من تأكيد الرقم السري كما هو");
                }

                // Generate New UserName
                string userName = GenerateUserName();
                // Generate New OTP
                string otp = GenerateOTP();
                // Add New user To Database
                RasedUser user = new RasedUser
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    UserName = userName,
                    OTP = otp,
                    AccountStatus = AccountStatusConstants.SUSPENDED,
                    OtpExpiryTime = DateTime.Now.AddMinutes(2),
                    CreatedAt = DateTime.Now
                };
                // Create User
                var result = await _userManager.CreateAsync(user, registerDto.Password);
                if (!result.Succeeded)
                {
                    return new ApiResponse<string>(result.Errors.Select(d => d.Description).ToList());
                }
                // Add To Role 
                var roleResult = await _userManager.AddToRoleAsync(user, RasedRolesConstants.User);
                if (!roleResult.Succeeded)
                {
                    return new ApiResponse<string>(roleResult.Errors.Select(d => d.Description).ToList());
                }

                // Send An OTP to the Email
                var sendEmail = await SendOtpByEmail(user.Email, user.FullName, otp);
                if (!sendEmail.IsSucceeded)
                {
                    return new ApiResponse<string>("خطأ تقني!! ... " + sendEmail.Message);
                }

                // Result Response
                return new ApiResponse<string>(null!, $"أهلا '{user.FullName}' باقي خطوة واحدة للإستمتاع 🚀");
            }
            catch(Exception ex)
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }

        // Login
        public async Task<ApiResponse<string>> LoginAsync(LoginDto loginDto)
        {
            try
            {
                // Check the User
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                if (user == null)
                {
                    return new ApiResponse<string>("خطأ في البريد الإلكتروني أو رقم المرور!");
                }
                // Check the password
                var checkPwd = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (!checkPwd)
                {
                    return new ApiResponse<string>("خطأ في البريد الإلكتروني أو رقم المرور!");
                }

                // >> NOTE <<
                // As long as the frontend reached this endpoint, we treat the request as it is for InActive Account

                // Check if the user is banned
                if (user.IsBanned)
                {
                    // Check if the Broke Date has value
                    if (user.BanBrokeAt == null || user.BanBrokeAt.Value > DateTime.Now)
                    {
                        return new ApiResponse<string>("تم حظر حسابك، يرجي فحص البريد الإلكتروني لتفاصيل أكثر");
                    }
                    else
                    {
                        // ---> Update banning data (Should be tasked by the system)
                        user.IsBanned = false;
                        user.BanBrokeAt = null;
                        user.BannedDuration = null;
                        user.BannedReason = null;
                        var banData = await _userManager.UpdateAsync(user);
                        if (!banData.Succeeded)
                        {
                            return new ApiResponse<string>(banData.Errors.Select(d => d.Description).ToList());
                        }
                    }
                }

                // Send An OTP to the Email
                string otp = GenerateOTP();
                var sendEmail = await SendOtpByEmail(user.Email!, user.FullName, otp);
                if (!sendEmail.IsSucceeded)
                {
                    return new ApiResponse<string>("خطأ تقني!! ... " + sendEmail.Message);
                }

                // Update Account Status and OTP
                user.AccountStatus = AccountStatusConstants.SUSPENDED;
                user.OTP = otp;
                user.OtpExpiryTime = DateTime.Now.AddMinutes(2);
                var accStat = await _userManager.UpdateAsync(user);
                if (!accStat.Succeeded)
                {
                    return new ApiResponse<string>(accStat.Errors.Select(d => d.Description).ToList());
                }

                // Result Response
                return new ApiResponse<string>(null!, $"أهلا '{user.FullName}' باقي خطوة واحدة للإستمتاع 🚀");
            }
            catch( Exception ex )
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }

        // Verify The Account
        public async Task<ApiResponse<AuthResponseDto>> VerifyOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            var result = new AuthResponseDto();
            try
            {
                // Email Check
                var user = await _userManager.FindByEmailAsync(verifyOtpDto.Email);
                if (user == null)
                {
                    result.HasEmailError = true;
                    return new ApiResponse<AuthResponseDto>(result, "خطأ ما حدث، حاول تسجيل الدخول مرة أخري!");
                }
                // Check if the user has been banned
                if (user.IsBanned)
                {
                    result.IsBanned = true;
                    return new ApiResponse<AuthResponseDto>(result, "تم حظر حسابك، يرجى فحص البريد الإلكتروني لتفاصيل أكثر.");
                }
                // Check the user OTP
                if (string.IsNullOrEmpty(user.OTP) || !user.OtpExpiryTime.HasValue)
                {
                    return new ApiResponse<AuthResponseDto>("خطأ تقني!");
                }
                // OTP Check
                if (user.OTP != verifyOtpDto.OTP)
                {
                    // Here The Block Logic Comes in ...
                    var blockResult = await HandleBlockUserAccountAsync(user);
                    if (!blockResult.IsSucceeded)
                    {
                        return new ApiResponse<AuthResponseDto>("خطأ تقني!" + blockResult.Message!);
                    }
                    if (blockResult.IsBanned)
                    {
                        result.IsBanned = true;
                        return new ApiResponse<AuthResponseDto>(result, "تم حظر حسابك، يرجى فحص البريد الإلكتروني لتفاصيل أكثر.");
                    }

                    // The Attempts incremented by one
                    return new ApiResponse<AuthResponseDto>("رمز التحقق غير صحيح، تأكد من إدخاله بشكل صحيح.");
                }
                // Expired Check
                if (DateTime.Now > user.OtpExpiryTime.Value)
                {
                    return new ApiResponse<AuthResponseDto>("انتهت صلاحية رمز التحقق، قم بالضغط علي إعادة إرسال واحد آخر!");
                }

                // ===>> Successful Verification <===
                // [1] Update User Data
                user.EmailConfirmed = true;
                user.IsBanned = false;
                user.FailedAttempts = 0;
                user.BanBrokeAt = null;
                user.BannedDuration = null;
                user.BannedReason = null;
                user.OTP = null;
                user.OtpExpiryTime = null;

                string message = "";
                // [2] Check the Account Status
                if (user.AccountStatus == AccountStatusConstants.SUSPENDED)
                {
                    user.AccountStatus = AccountStatusConstants.ACTIVE;
                    // Generate The Access Token
                    #region Claims
                    List<Claim> claims = new List<Claim>()
                    {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // User ID as Subject
                    new Claim(JwtRegisteredClaimNames.Email, user.Email!), // User Email
                    new Claim(JwtRegisteredClaimNames.Name, user.FullName), // User FullName
                    new Claim(JwtRegisteredClaimNames.Typ, user.UserBadge), // User Badge
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())// Token Identifier
                    };
                    // Roles As Claims
                    var UserRoles = await _userManager.GetRolesAsync(user);
                    foreach (var role in UserRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }
                    #endregion

                    result.AccessToken = GenerateToken(claims, verifyOtpDto.RememberMe);
                    message = "مرحبًا، سعداء بوجودك معنا 🎉";
                }
                else
                {
                    user.AccountStatus = AccountStatusConstants.RESETPWD;
                    message = "تم التحقق من حسابك بنجاح، يمكنك الآن إعادة تعيين كلمة المرور الخاصة بك.";
                }
                // Update User Data
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    return new ApiResponse<AuthResponseDto>(updateUser.Errors.Select(d => d.Description).ToList());
                }                
                
                // [3] Response
                result.AccountStatus = user.AccountStatus;
                return new ApiResponse<AuthResponseDto>(result, message);
            }
            catch( Exception ex )
            {
                return new ApiResponse<AuthResponseDto>("خطأ تقني! " + ex.Message);
            }
        }

        // Resend Otp
        public async Task<ApiResponse<string>> ResendOtpAsync(ResendOtpDto resendOtpDto)
        {
            try
            {
                // Check the Email
                var user = await _userManager.FindByEmailAsync(resendOtpDto.Email);
                if (user == null)
                {
                    return new ApiResponse<string>("خطأ تقني! ... يرجي تسجيل الدخول مرة أخري");
                }
                // Check if the account status is suspended
                if (user.AccountStatus != AccountStatusConstants.SUSPENDED && user.AccountStatus != AccountStatusConstants.RESETPWD)
                {
                    return new ApiResponse<string>("خطأ تقني! ... يرجي تسجيل الدخول مرة أخري");
                }

                // New OTP
                string otp = GenerateOTP();
                // Update User Data
                user.OTP = otp;
                user.OtpExpiryTime = DateTime.Now.AddMinutes(2);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ApiResponse<string>(updateResult.Errors.Select(d => d.Description).ToList());
                }

                // Send An OTP to the Email
                bool isPwd = false;
                // Check if the resending is for Reset The Password
                if (user.AccountStatus == AccountStatusConstants.RESETPWD)
                    isPwd = true;
                var sendEmail = await SendOtpByEmail(user.Email!, user.FullName, otp, isPwd);
                if (!sendEmail.IsSucceeded)
                {
                    return new ApiResponse<string>("خطأ تقني!! ... " + sendEmail.Message);
                }

                return new ApiResponse<string>(null!, "تم إرسال رمز التحقق إلى بريدك الإلكتروني!");
            }
            catch(Exception ex )
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }

        // Reset Password
        public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            try
            {
                // Check User
                var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
                if (user == null)
                {
                    return new ApiResponse<string>("خطأ في البريد الإلكتروني، حاول مرة أخري!");
                }
                // Check if the account is active
                if (user.AccountStatus == AccountStatusConstants.ACTIVE)
                {
                    return new ApiResponse<string>(null!, "يمكنك الإستمتاع الآن، اذهب إلي الصفحة الرئيسية!");
                }
                // Check if the user is banned
                if (user.IsBanned)
                {
                    // Check if the Broke Date has value
                    if (user.BanBrokeAt == null || user.BanBrokeAt.Value > DateTime.Now)
                    {
                        return new ApiResponse<string>("تم حظر حسابك، يرجي فحص البريد الإلكتروني لتفاصيل أكثر");
                    }
                    else
                    {
                        // ---> Update banning data (Should be tasked by the system)
                        user.IsBanned = false;
                        user.BanBrokeAt = null;
                        user.BannedDuration = null;
                        user.BannedReason = null;
                        var banData = await _userManager.UpdateAsync(user);
                        if (!banData.Succeeded)
                        {
                            return new ApiResponse<string>(banData.Errors.Select(d => d.Description).ToList());
                        }
                    }
                }

                // OTP
                string otp = GenerateOTP();
                // Update User Data
                user.AccountStatus = AccountStatusConstants.RESETPWD;
                user.OTP = otp;
                user.OtpExpiryTime = DateTime.Now.AddMinutes(2);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ApiResponse<string>(updateResult.Errors.Select(d => d.Description).ToList());
                }

                // Sending OTP with Email
                var sendEmail = await SendOtpByEmail(user.Email!, user.FullName, otp, true);
                if (!sendEmail.IsSucceeded)
                {
                    return new ApiResponse<string>("خطأ تقني!! ... " + sendEmail.Message);
                }

                return new ApiResponse<string>(null!, "تم إرسال رمز التحقق إلى بريدك الإلكتروني!");
            }
            catch( Exception ex )
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }

        // Reset Password
        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                // Check User
                var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
                if (user == null)
                {
                    return new ApiResponse<string>("خطأ ما حدث، حاول تسجيل الدخول مرة أخري!");
                }
                // Check the account should be RESETPWD
                if (user.AccountStatus != AccountStatusConstants.RESETPWD)
                {
                    return new ApiResponse<string>("خطأ ما حدث، حاول تسجيل الدخول مرة أخري!");
                }
                // Check Password
                if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmedNewPassword)
                {
                    return new ApiResponse<string>("لابد من تأكيد الرقم السري كما هو");
                }

                // Reset Passowrd
                var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, ResetToken, resetPasswordDto.NewPassword);
                if (!result.Succeeded)
                {
                    return new ApiResponse<string>(result.Errors.Select(d => d.Description).ToList());
                }

                // Update User Data
                user.AccountStatus = AccountStatusConstants.INACTIVE;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ApiResponse<string>(updateResult.Errors.Select(d => d.Description).ToList());
                }

                // Send an email to tell the user that the password has been changed
                string emailSubject = "🔐 نجحت ــ إعادة تعيين كلمة المرور";
                string emailBody = $@"
                <p>مرحبًا <strong>{user.FullName}</strong>،</p>
                <p>
                    تم إعادة تعيين كلمة المرور الخاصة بك بنجاح. 🎉
                </p>
                <p style='margin-top: 25px; color: #555; font-size: 14px;'>
                    مع تحيات فريق <strong>راصِــــــد</strong> 💰
                </p>
                ";
                var sendEmail = await _emailService.SendEmailAsync(user.Email!, emailSubject, emailBody);
                if (!sendEmail.successed)
                {
                    return new ApiResponse<string>("خطأ تقني!! ... " + sendEmail.Message);
                }

                return new ApiResponse<string>(null!, "تم إعادة تعيين كلمة المرور بنجاح 🎉!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }

        // Logout User
        public async Task<ApiResponse<string>> LogoutAsync(LogoutDto model)
        {
            try
            {
                // Check User
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    return new ApiResponse<string>("خطأ ما حدث، حاول مرة أخري!");
                }
                // Check if the user is active
                if (user.AccountStatus != AccountStatusConstants.ACTIVE)
                {
                    return new ApiResponse<string>("خطأ ما حدث، حاول مرة أخري!");
                }
                // Update User Data
                user.AccountStatus = AccountStatusConstants.INACTIVE;
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return new ApiResponse<string>(updateResult.Errors.Select(d => d.Description).ToList());
                }

                return new ApiResponse<string>(null!, "تم تسجيل الخروج بنجاح!");
            }
            catch (Exception ex)
            {
                return new ApiResponse<string>("خطأ تقني!! ... " + ex.Message);
            }
        }


        // Generate UserName
        private string GenerateUserName()
        {
            string name = "rased-user-";
            return $"{name}{new Random().Next(1000, 9999)}";
        }
        // Generate OTP
        private string GenerateOTP()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }
        // Send An OTP
        private async Task<StatusDto> SendOtpByEmail(string email, string fullName, string otp, bool isPwd = false)
        {
            var result = new StatusDto();

            // Email Content
            string emailSubject = "🔐 رمز التحقق لمرة واحدة";
            if (isPwd)
                emailSubject = "🔐 رمز التحقق لمرة واحدة - إعادة تعيين كلمة المرور";
            string emailBody = $@"
            <p>مرحبًا <strong>{fullName}</strong>،</p>

            <p>
                رمز التحقق (OTP) الخاص بك هو:
            </p>

            <p style='font-size: 20px; color: #2c3e50; background-color: #f2f2f2; padding: 10px; border-radius: 5px; text-align: center; width: fit-content;'>
                🔹 <strong>{otp}</strong>
            </p>

            <p>⏰ هذا الرمز صالح لمدة <strong>2 دقيقتان</strong> فقط.</p>

            <p style='color: #c0392b;'>⚠️ الرجاء عدم مشاركة هذا الرمز مع أي شخص حفاظًا على أمان حسابك.</p>

            <p>إذا لم تطلب هذا الرمز، يمكنك تجاهل هذه الرسالة.</p>

            <p style='margin-top: 25px; color: #555; font-size: 14px;'>
                مع تحيات فريق <strong>راصِــــــد</strong> 💰
            </p>
            ";
            var sendEmail = await _emailService.SendEmailAsync(email, emailSubject, emailBody);
            if(!sendEmail.successed)
            {
                result.Message = $"خطأ تقني ... {sendEmail.Message}";
                return result;
            }

            result.IsSucceeded = true;
            return result;
        }
        // Handle Block Account Logic
        private async Task<BanAccountDto> HandleBlockUserAccountAsync(RasedUser user)
        {
            var result = new BanAccountDto();

            // Every 3 wrong attempts a Ban Strategy applied
            try
            {
                // Increment the Failed Attempts
                user.FailedAttempts += 1;
                int attempts = user.FailedAttempts;
                // Check for Alerts
                if (attempts != 0 && attempts % 3 == 0)
                {
                    user.IsBanned = true;
                    user.AccountStatus = AccountStatusConstants.INACTIVE;
                    string emailSubject = "🚫 إشعار بحظر الحساب";
                    string emailBody = "";
                    string banDurationText = "";

                    if (attempts / 3 == 1)
                    {
                        user.BannedReason = $"الكثير من المحاولات الخاطئة مما أدي لحظر الحساب لمدة [{BanAcountConstants.QUARTER}]";
                        user.BannedDuration = BanAcountConstants.QUARTER;
                        user.BanBrokeAt = DateTime.Now.AddMinutes(15);
                        banDurationText = "15 دقيقة";
                    }
                    else if (attempts / 3 == 2)
                    {
                        user.BannedReason = $"الكثير من المحاولات الخاطئة مما أدي لحظر الحساب لمدة [{BanAcountConstants.ONE_HOUR}]";
                        user.BannedDuration = BanAcountConstants.ONE_HOUR;
                        user.BanBrokeAt = DateTime.Now.AddHours(1);
                        banDurationText = "ساعة واحدة";
                    }
                    else if (attempts / 3 == 3)
                    {
                        user.BannedReason = $"الكثير من المحاولات الخاطئة مما أدي لحظر الحساب لمدة [{BanAcountConstants.ONE_DAY}]";
                        user.BannedDuration = BanAcountConstants.ONE_DAY;
                        user.BanBrokeAt = DateTime.Now.AddDays(1);
                        banDurationText = "يوم كامل";
                    }
                    else
                    {
                        user.BannedReason = $"الكثير من المحاولات الخاطئة مما أدي لحظر الحساب لمدة [{BanAcountConstants.FOREVER}]";
                        user.BannedDuration = BanAcountConstants.FOREVER;
                        user.BanBrokeAt = null;
                        banDurationText = "غير محدد (حظر دائم)";
                    }

                    // Email Body (HTML)
                    emailBody = $@"
                    <p>مرحبًا {user.FullName}،</p>

                    <p style='color: #b22222;'>
                        🚫 نأسف لإبلاغك بأنه قد تم <strong>حظر حسابك</strong> بسبب العديد من محاولات تسجيل الدخول الخاطئة.
                    </p>

                    <p><strong>مدة الحظر:</strong> {banDurationText}</p>

                    <p style='margin-top: 15px;'>
                        يمكنك محاولة تسجيل الدخول مجددًا بعد انتهاء مدة الحظر المحددة.
                    </p>

                    <p style='color: #888; font-size: 14px; margin-top: 25px;'>
                        إذا كنت تعتقد أن هذا تم عن طريق الخطأ، يمكنك التواصل مع فريق الدعم.
                    </p>

                    <p style='margin-top: 30px;'>
                        مع تحيات فريق <strong>راصِــــــد</strong> 🤝
                    </p>
                    ";
                    // Send Email
                    await _emailService.SendEmailAsync(user.Email!, emailSubject, emailBody);
                    // Response
                    result.IsBanned = true;
                }

                // Update User Data
                var updateUser = await _userManager.UpdateAsync(user);
                if (!updateUser.Succeeded)
                {
                    result.Message = updateUser.Errors.Select(d => d.Description + ", ").ToString();
                }
                // Response
                result.IsSucceeded = true;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
            }

            return result;
        }
        // Generate New JWT Token
        private string GenerateToken(IList<Claim> claims, bool RememberMe)
        {

            #region Token
            #region SecurityKey
            var SecretKeyString = _configuration.GetSection("SecretKey").Value;
            var SecretKeyByte = Encoding.ASCII.GetBytes(SecretKeyString);
            SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);
            #endregion

            //Combind SecretKey , HasingAlgorithm (SigningCredentials)
            SigningCredentials signingCredential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //Determine Expiration
            DateTime tokenExpiration = RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddDays(1);

            //Token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: signingCredential,
                expires: tokenExpiration
            );
            //To convert Token To String
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            string token = handler.WriteToken(jwtSecurityToken);
            #endregion

            return token;
        }


        // Old Code
        /*
         private string GenerateRefreshToken()
        {
            var randomBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }
            return Convert.ToBase64String(randomBytes);
        }

        public async Task<LoginResponce> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var response = new LoginResponce();

            //   Extract Info From ExpiredToken
            var principal = GetPrincipalFromExpiredToken(refreshTokenDto.Token);
            if (principal == null)
            {
                response.Errors.Add("Invalid access token.");
                return response;
            }
            // Get User By Id That Exist In Token
            var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != refreshTokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                response.Errors.Add("Invalid refresh token.");
                return response;
            }

            //  Generate New Token And  Refresh Token 
            var newAccessToken = GenerateToken(principal.Claims.ToList(), false);
            var newRefreshToken = GenerateRefreshToken();

            //   Update User Data
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15);
            await _userManager.UpdateAsync(user);

            response.successed = true;
            response.Token = newAccessToken;
            response.RefreshToken = newRefreshToken;
            return response;
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // Retrieve the secret key and Convert it into a byte array for decryption
            var secretKey = _configuration.GetSection("SecretKey").Value;
            var key = Encoding.ASCII.GetBytes(secretKey);

            // Define token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true, // Ensure the token is signed with the correct key and Set the signing key
                IssuerSigningKey = new SymmetricSecurityKey(key), 

    
                ValidateIssuer = false, // Do not validate the token issuer and   audience
                ValidateAudience = false, 
                ValidateLifetime = false // Ignore token expiration 
            };

            //  Create a JwtSecurityTokenHandler instance to validate and decode the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            // Ensure the token is a valid JWT and was signed using HmacSha256
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token."); 
            }

            // Return the principal (user identity and claims) 
            return principal;
        }
         */
    }
}
