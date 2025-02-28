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
        Task<LoginResponce> Login(LoginDto loginDto);
        Task<LoginResponce> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);

        Task<GeneralRespnose> Register(RegisterDto registerDto);
        Task<GeneralRespnose> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);


        Task<GeneralRespnose> ForgotPassword(ForgotPasswordDto forgotPasswordDto);
        Task<GeneralRespnose> ResetPassword(ResetPasswordDto resetPasswordDto);


    }
}
