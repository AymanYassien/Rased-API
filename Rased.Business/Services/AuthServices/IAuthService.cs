using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Auths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<LoginResponce> LoginAsync(LoginDto loginDto);
        Task<LoginResponce> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);

        Task<GeneralRespnose> RegisterAsync(RegisterDto registerDto);
        Task<GeneralRespnose> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);


        Task<GeneralRespnose> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<GeneralRespnose> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

        Task<GeneralRespnose> ResendOtpAsync(ResendOtpDto resendOtpDto);


    }
}
