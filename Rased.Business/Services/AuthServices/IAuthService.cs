using Rased.Business.Dtos.Auths;
using Rased.Business.Dtos.Response;

namespace Rased.Business.Services.AuthServices
{
    public interface IAuthService
    {
        Task<ApiResponse<string>> RegisterAsync(RegisterDto registerDto);
        Task<ApiResponse<string>> LoginAsync(LoginDto loginDto);
        Task<ApiResponse<AuthResponseDto>> VerifyOtpAsync(VerifyOtpDto verifyOtpDto);
        Task<ApiResponse<string>> ResendOtpAsync(ResendOtpDto resendOtpDto);
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
        Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
        Task<ApiResponse<string>> LogoutAsync(LogoutDto model);

        //Task<LoginResponce> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
