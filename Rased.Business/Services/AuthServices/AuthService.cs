using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rased.Business.Dtos.Auths;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<GeneralRespnose> RegisterAsync(RegisterDto registerDto)
        {
            var response = new GeneralRespnose();
            if (_userManager.Users.Any(s => s.FirstName == registerDto.FirstName && s.LastName == registerDto.LastName))
            {
                response.Errors.Add("This name is already taken. Please choose another one.");
                return response;
            }

            if (_userManager.Users.Any(s => s.Email == registerDto.Email))
            {
                response.Errors.Add("Email Is Already Exist");
                return response;

            }

            if (registerDto.Password != registerDto.ConfirmedPassword)
            {
                response.Errors.Add("Password and confirmation password do not match.");
                return response;
            }

            RasedUser user = new RasedUser 
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.FirstName + registerDto.LastName ,
                OTP = GenerateOTP(),
                CreatedAt = DateTime.UtcNow,
                OtpExpiryTime = DateTime.UtcNow.AddMinutes(10)
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                // OTP
                string emailBody = $"Dear {user.FirstName} {user.LastName},\r\n\r\n" +
                          $"Your one-time password (OTP) for verification is:\r\n\r\n" +
                          $"🔹 {user.OTP}\r\n\r\n" +
                          $"This code is valid for 10 minutes. Please do not share this code with anyone for security reasons.\r\n\r\n" +
                          $"If you didn't request this code, please ignore this email.\r\n\r\n" +
                          $"Best regards,\r\n" +
                          $"The Rased Team";
                await _emailService.SendEmailAsync(user.Email, "Verify Your Email - Rased Project", emailBody);

                // Assign the role 
                await _userManager.AddToRoleAsync(user, RasedRolesConstants.User);

                response.successed = true;
                return response;

            }

            response.Errors = result.Errors.Select(d => d.Description).ToList();
            return response;

        }

        public async Task<GeneralRespnose> VerifyOtpAsync(VerifyOtpDto verifyOtpDto)
        {
            var response = new GeneralRespnose();

            var user = await _userManager.FindByEmailAsync(verifyOtpDto.Email);
            if (user == null)
            {
                response.Errors.Add("Email not found. Please make sure the email is correct.");
                return response;
            }

            if (string.IsNullOrEmpty(user.OTP) || user.OTP != verifyOtpDto.OTP)
            {
                response.Errors.Add("Invalid OTP.");
                return response;
            }

            if (!user.OtpExpiryTime.HasValue || DateTime.UtcNow > user.OtpExpiryTime.Value)
            {
                response.Errors.Add("OTP has expired. Please request a new one.");
                return response;
            }

            user.EmailConfirmed = true;
            user.OTP = null;
            user.OtpExpiryTime = null;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            response.successed = true;
            return response;
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }


        // Login
        public async Task<LoginResponce> LoginAsync(LoginDto loginDto)
        {
            var response = new LoginResponce();
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
            {
                response.Errors.Add(user == null ? "Email not found. Please make sure the email is correct." :
                   "Email not confirmed. Please check your inbox.");
                return response;
            }

            if (user.IsBanned)
            {
                response.Errors.Add("Your account has been banned.");
                return response;
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (result)
            {
                #region Claims
                List<Claim> claims = new List<Claim>()
                {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // User ID as Subject
                new Claim(JwtRegisteredClaimNames.Email, user.Email), // User Email
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())// Token Identifier
                };
                var UserRoles = await _userManager.GetRolesAsync(user);
                foreach (var role in UserRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }
                #endregion



                var existingClaims = await _userManager.GetClaimsAsync(user);
                if (!existingClaims.Any())
                {
                    await _userManager.AddClaimsAsync(user, claims);
                }
                response.Token = GenerateToken(claims, loginDto.RememberMe);

                //  Generate Refresh Token
                response.RefreshToken = GenerateRefreshToken();

                //  Save Refresh Token  
                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(15); 
                await _userManager.UpdateAsync(user);

                response.successed = true;
                return response;

            }
            response.Errors.Add("Wrong Password or Email");
            return response;

        }

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
            DateTime tokenExpiration = RememberMe ? DateTime.UtcNow.AddHours(24) : DateTime.UtcNow.AddHours(1);

            //Token
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken
            (
                // issuer: _configuration["Jwt:Issuer"],  
                //audience: _configuration["Jwt:Audience"],  
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



       // Reset Password
        public async Task<GeneralRespnose> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
        {
            var response = new GeneralRespnose();
            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);

            if (user == null)
            {
                response.Errors.Add("Email not found. Please make sure the email is correct.");
                return response;
            }

            user.OTP = GenerateOTP();
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);

            await _userManager.UpdateAsync(user);

            // Send OTP to user
            string emailBody = $"Dear {user.FirstName} {user.LastName},\r\n\r\n" +
                               $"Your password reset OTP is:\r\n\r\n" +
                               $"🔹 {user.OTP}\r\n\r\n" +
                               $"This code is valid for 5 minutes.\r\n\r\n" +
                               $"If you didn't request this, please ignore this email.\r\n\r\n" +
                               $"Best regards,\r\n" +
                               $"The Rased Team";

             var result = await _emailService.SendEmailAsync(user.Email, "Password Reset OTP - Rased Project", emailBody);

            if (result.successed)
            {
                response.successed = result.successed;
                return response;
            }

            response.Errors.AddRange(result.Errors);
            return response;
        }

        public async Task<GeneralRespnose> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var response = new GeneralRespnose();
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null || user.OTP != resetPasswordDto.OTP)
            {
                response.Errors.Add("Invalid OTP or Email.");
                return response;
            }

            if (user.OtpExpiryTime == null || DateTime.UtcNow > user.OtpExpiryTime)
            {
                response.Errors.Add("OTP has expired. Please request a new one.");
                return response;
            }

            if (resetPasswordDto.NewPassword != resetPasswordDto.ConfirmedNewPassword)
            {
                response.Errors.Add("New password and confirmation password do not match.");
                return response;
            }

            // Reset Passowrd
            var ResetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, ResetToken, resetPasswordDto.NewPassword);

            if (!result.Succeeded)
            {
                response.Errors.AddRange(result.Errors.Select(e => e.Description));
                return response;
            }

            user.OTP = null;
            user.OtpExpiryTime = null;
            await _userManager.UpdateAsync(user);

            response.successed = true;
            return response;
        }


        // Resend Otp
        public async Task<GeneralRespnose> ResendOtpAsync(ResendOtpDto resendOtpDto)
        {
            var response = new GeneralRespnose();

            var user = await _userManager.FindByEmailAsync(resendOtpDto.Email);
            if (user == null)
            {
                response.Errors.Add("Email not found. Please make sure the email is correct.");
                return response;
            }

            if (!resendOtpDto.isForResetPassword && user.EmailConfirmed)
            {
                response.Errors.Add("Email is already verified.");
                return response;
            }

            // New OTP
            user.OTP = GenerateOTP();
            user.OtpExpiryTime = DateTime.UtcNow.AddMinutes(10);

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                response.Errors.AddRange(updateResult.Errors.Select(e => e.Description));
                return response;
            }

            // Register Or Reset Password
            string subject, emailBody;

            if (resendOtpDto.isForResetPassword)
            {
                subject = "Password Reset OTP - Rased Project";
                emailBody = $"Dear {user.FirstName} {user.LastName},\r\n\r\n" +
                            $"Your password reset OTP is:\r\n\r\n" +
                            $"🔹 {user.OTP}\r\n\r\n" +
                            $"This code is valid for 10 minutes.\r\n\r\n" +
                            $"If you didn't request this, please ignore this email.\r\n\r\n" +
                            $"Best regards,\r\n" +
                            $"The Rased Team";
            }
            else
            {
                subject = "Verify Your Email - Rased Project";
                emailBody = $"Dear {user.FirstName} {user.LastName},\r\n\r\n" +
                            $"Your new OTP for email verification is:\r\n\r\n" +
                            $"🔹 {user.OTP}\r\n\r\n" +
                            $"This code is valid for 10 minutes. Please do not share this code with anyone for security reasons.\r\n\r\n" +
                            $"If you didn't request this, please ignore this email.\r\n\r\n" +
                            $"Best regards,\r\n" +
                            $"The Rased Team";
            }

            var emailResult = await _emailService.SendEmailAsync(user.Email, subject, emailBody);

            if (emailResult.successed)
            {
                response.successed = true;
                return response;
            }

            response.Errors.AddRange(emailResult.Errors);
            return response;
        }




    }
}
