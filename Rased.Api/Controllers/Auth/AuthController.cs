using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Auths;
using Rased.Business.Services.AuthServices;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        // Register
        [HttpPost("register")]
        public async Task<IActionResult> Register( [FromBody] RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            if (!result.successed)
                return BadRequest(result.Errors);

            return Ok(result.successed);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody]  VerifyOtpDto verifyOtpDto)
        {
            var result = await _authService.VerifyOtpAsync(verifyOtpDto);
            if (!result.successed)
                return BadRequest(result.Errors);

            return Ok(result.successed);
        }



        // Login
        [HttpPost("login")]
        public async Task<IActionResult> Login( [FromBody] LoginDto loginDto)
        {
            var response = await _authService.LoginAsync(loginDto);

            if (!response.successed)
                return BadRequest(response.Errors);    

            return Ok(new {response.successed, response.Token, response.RefreshToken });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken( [FromBody] RefreshTokenDto refreshTokenDto)
        {
            var response = await _authService.RefreshTokenAsync(refreshTokenDto);

            if (!response.successed)
                return BadRequest(response.Errors);

            return Ok( new {response.successed, token = response.Token, refreshToken = response.RefreshToken });
        }


        // Reset Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword( [FromBody] ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);
            if (!result.successed)
                return BadRequest( result.Errors );

            return Ok(new { success = true, message = "OTP has been sent successfully to your email." });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordDto);
            if (!result.successed)
                return BadRequest(result.Errors );

            return Ok(new { success = true, message = "Your password has been reset successfully." });
        }

        // Resend OTP
        [HttpPost("ResendOtp")]
        public async Task<IActionResult> ResendOtp( [FromBody] ResendOtpDto resendOtpDto)
        {
      
            var result = await _authService.ResendOtpAsync(resendOtpDto);

            if (!result.successed)
            {
                return BadRequest(new { errors = result.Errors });
            }

            return Ok(new { message = "OTP has been resent successfully." });
        }


    }
}
