using System.Net;
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
        private readonly IEmailService _emailService;

        public AuthService(UserManager<ApplicationUser> userManager,
                           SignInManager<ApplicationUser> signInManager,
                           IJwtService jwtService,
                           IDtoMapper DtoMapper,
                           IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtService = jwtService;
            _DtoMapper = DtoMapper;
            _emailService = emailService;
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

            await _userManager.AddToRoleAsync(user, "User"); // Assign default role

            var code = await _userManager.GenerateTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider);

            var emailBody = $"<h1>Welcome to I-Clinic!</h1>" +
                      $"<p>Your email confirmation code is:</p>" +
                      $"<h2 style='font-size: 36px; letter-spacing: 4px;'>{code}</h2>" +
                      $"<p>This code will expire in 10 minutes.</p>";

            await _emailService.SendEmailAsync(user.Email!, "Confirm Your Email Code", emailBody);

            return new AuthResponseDto
            {
                Message = "Registration successful. Please check your email for your confirmation code.",
                Status = true,
                Code = 200
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
            if (!user.EmailConfirmed)
            {
                return new AuthResponseDto
                {
                    Message = "Please confirm your email before logging in.",
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
                },
                Errors = ["no errors"]
            };
        }
        public async Task<AuthResponseDto> ConfirmEmailWithCodeAsync(ConfirmEmailDto confirmCodeDto)
        {
            var user = await _userManager.FindByEmailAsync(confirmCodeDto.Email);
            if (user == null)
            {
                return new AuthResponseDto { Status = false, Message = "Invalid email address." };
            }

            // Use the same method to verify the code. Identity knows how to handle it.
            var isValid = await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultEmailProvider, confirmCodeDto.Code);

            if (!isValid)
            {
                return new AuthResponseDto
                {
                    Status = false,
                    Message = "Invalid confirmation code."
                };
            }

            user.EmailConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return new AuthResponseDto
                {
                    Status = false,
                    Message = "Failed to confirm email.",
                    Errors = updateResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // If confirmation is successful, you might want to return a login token immediately
            // to improve the user experience, so they don't have to log in again.
            var token = await _jwtService.GenerateTokenAsync(user);

            return new AuthResponseDto
            {
                Status = true,
                Message = "Email confirmed successfully!",
                Data = new AuthTokenDto
                {
                    Token = token,
                    Username = user.Email!
                }
            };
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

            return Task.FromResult(true);
        }
    }

}


    
