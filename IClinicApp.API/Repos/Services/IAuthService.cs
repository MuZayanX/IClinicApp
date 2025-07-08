using IClinicApp.API.Dtos.Auth;

namespace IClinicApp.API.Repos.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task LogoutAsync();
        Task<AuthResponseDto> ConfirmEmailWithCodeAsync(ConfirmEmailDto confirmCodeDto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
