using IClinicApp.API.Dtos.UserProfile;

namespace IClinicApp.API.Repos.Services
{
    public interface IUserProfileService
    {
        Task<UserProfileDto> GetUserProfileAsync(Guid userId);
        Task<bool> UpdateUserProfileAsync(UpdateUserProfileDto dto);
    }
}
