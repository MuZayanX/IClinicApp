using IClinicApp.API.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using IClinicApp.API.Dtos.Auth;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using IClinicApp.API.Repos.Implementations;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;

namespace IClinicApp.API.Controllers
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

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);
            if (!result.Status)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDto dto)
        {
            var result = await _authService.ConfirmEmailAsync(dto);
            if (!result)
                return BadRequest("Invalid token or user.");

            return Ok("Email confirmed successfully.");
        }
        [Authorize]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            if (!result && !string.IsNullOrWhiteSpace(dto.Token))
                return BadRequest("Password reset failed.");

            return Ok(string.IsNullOrWhiteSpace(dto.Token)
                ? "Password reset token generated and (normally) sent via email."
                : "Password has been reset successfully.");
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.LogoutAsync();
            return Ok("Logged out successfully.");
        }

        [Authorize]
        [HttpGet("get-hello")]
        public IActionResult GetHello()
        {
            return Ok("Hello from AuthController!");
        }
    }
}
