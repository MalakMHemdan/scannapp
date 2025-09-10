using BLL.DTOs;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace scannapp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController2 : ControllerBase
    {
        private readonly ILoginService _loginService;
        private readonly IRegisterService _registerService;
        private readonly IConfirmEmailService _confirmEmailService;
        private readonly IForgotPasswordService _forgotPasswordService;
        private readonly IResetPasswordService _resetPasswordService;
        private readonly IChangePasswordService _changePasswordService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController2(
            ILoginService loginService,
            IRegisterService registerService,
            IConfirmEmailService confirmEmailService,
            IForgotPasswordService forgotPasswordService,
            IResetPasswordService resetPasswordService,
            IChangePasswordService changePasswordService,
            IRefreshTokenService refreshTokenService)
        {
            _loginService = loginService;
            _registerService = registerService;
            _confirmEmailService = confirmEmailService;
            _forgotPasswordService = forgotPasswordService;
            _resetPasswordService = resetPasswordService;
            _changePasswordService = changePasswordService;
            _refreshTokenService = refreshTokenService;
        }

        // 1. Register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            var result = await _registerService.RegisterAsync(model);
            return Ok(result);
        }

        // 2. Login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var token = await _loginService.LoginAsync(model);
            if (token == null) return Unauthorized("Invalid username or password");
            return Ok(token); //  Refresh Token
        }

        // 3. Confirm Email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] int userId, [FromQuery] string token)
        {
            var result = await _confirmEmailService.ConfirmEmailAsync(userId, token);
            return Ok(result);
        }

        // 4. Forgot Password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _forgotPasswordService.ForgotPasswordAsync(email);
            return Ok(result);
        }

        // 5. Reset Password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var result = await _resetPasswordService.ResetPasswordAsync(model);
            return Ok(result);
        }

        // 6. Change Password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto model)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _changePasswordService.ChangePasswordAsync(model, userId);
            return Ok(result);
        }

        // 7. Refresh Token
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto model)
        {
            var result = await _refreshTokenService.RefreshTokenAsync(model.RefreshToken);
            if (result == null) return Unauthorized("Invalid or expired refresh token");
            return Ok(result);
        }
    }
}
