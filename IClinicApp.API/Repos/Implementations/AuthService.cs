using AutoMapper;
using IClinicApp.API.Dtos.Auth;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;

namespace IClinicApp.API.Repos.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtService _jwtService;
        private readonly IDtoMapper _DtoMapper;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IJwtService jwtService,
                           IDtoMapper DtoMapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _DtoMapper = DtoMapper;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = _DtoMapper.ToApplicationUser(registerDto);
            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Message = "Registration failed.",
                    Status = false,
                    Code = 400,
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            var token = await _jwtService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Message = "Registered Successfully.",
                Status = true,
                Code = 200,
                Data = new AuthTokenDto
                {
                    Token = token,
                    Username = user.UserName!
                }
            };
        }
        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            // 1. Check if user exists by email
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    Message = "Invalid credentials.",
                    Status = false,
                    Code = 401
                };
            }

            // 2. Check if password is correct
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded)
            {
                return new AuthResponseDto
                {
                    Message = "Invalid credentials.",
                    Status = false,
                    Code = 401
                };
            }

            // 3. Generate JWT token
            var token = await _jwtService.GenerateTokenAsync(user);

            // 4. Return success response with token
            return new AuthResponseDto
            {
                Message = "Loggedin Successfully.",
                Status = true,
                Code = 200,
                Data = new AuthTokenDto
                {
                    Token = token,
                    Username = user.UserName!
                }
            };
        }
        public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmEmailDto.Email);

            if (user == null)
                return false;

            var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.Token);

            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
                return false;

            if (string.IsNullOrWhiteSpace(dto.Token))
            {
                // No token? Generate and send reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // You would send this via email - for demo, log it or return it
                Console.WriteLine($"Password reset token for {user.Email}: {token}");
                return true;
            }
            else
            {
                // Token provided? Reset password
                var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
                return result.Succeeded;
            }
        }

        public Task LogoutAsync()
        {
            // Since JWTs are stateless, logout just requires the client to remove the token.
            // You could log the logout event or add token blacklisting here in the future.

            // For now, just return a completed task.
            // This is a placeholder for any future logout logic.
            // For example, if you had a token blacklist, you could add the token to it here.
            // Or if you were using session-based authentication, you could invalidate the session.
            // But with JWT, the token is already stateless and doesn't require server-side invalidation.
            // So, we just return a completed task.

            return Task.CompletedTask;
        }
    }

}


    
