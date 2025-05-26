using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rased.Business.Dtos.Auths;
using Rased.Business.Services.AuthServices;
using System.Security.Claims;

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
        [HttpPost("Register", Name = "AuthRegister")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Account Verification
        [HttpPost("Otp/Verify", Name = "AuthVerify")]
        public async Task<IActionResult> VerifyOtp(VerifyOtpDto verifyOtpDto)
        {
            var result = await _authService.VerifyOtpAsync(verifyOtpDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Login
        [HttpPost("Login", Name = "AuthLogin")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Resend OTP
        [HttpPost("Otp/Resend", Name = "AuthResendOtp")]
        public async Task<IActionResult> ResendOtp(ResendOtpDto resendOtpDto)
        {

            var result = await _authService.ResendOtpAsync(resendOtpDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Forgot Password
        [HttpPost("Password/Forgot", Name = "AuthForgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var result = await _authService.ForgotPasswordAsync(forgotPasswordDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Reset Password
        [HttpPost("Password/Reset", Name = "AuthResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var result = await _authService.ResetPasswordAsync(resetPasswordDto);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Logout
        [HttpPost("Logout", Name = "AuthLogout")]
        public async Task<IActionResult> Logout(LogoutDto model)
        {
            var result = await _authService.LogoutAsync(model);

            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        // Get User
        [Authorize]
        [HttpGet("GetUser", Name = "GetUser")]
        public async Task<IActionResult> GetUser()
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _authService.GetUserAsync(curUserId!);
            if(!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }

        [Authorize]
        [HttpPut("UpdateUser", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto model)
        {
            // Current Authenticated User
            var curUserId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

            var result = await _authService.UpdateUserAsync(model, curUserId!);
            if (!result.Succeeded)
                return BadRequest(result);

            return Ok(result);
        }


        // RefreshToken
        /*
         [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            var response = await _authService.RefreshTokenAsync(refreshTokenDto);

            if (!response.successed)
                return BadRequest(response.Errors);

            return Ok(new
            {
                response.successed,
                token = response.Token,
                refreshToken = response.RefreshToken,
                Message = "Token refresh successful!"
            });
        }
         */
    }
}
