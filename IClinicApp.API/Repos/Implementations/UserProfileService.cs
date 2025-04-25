using IClinicApp.API.Data;
using IClinicApp.API.Dtos.UserProfile;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;

namespace IClinicApp.API.Repos.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IDtoMapper _dtoMapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserProfileService( IDtoMapper dtoMapper, UserManager<ApplicationUser> userManager)
        {
            _dtoMapper = dtoMapper;
            _userManager = userManager;
        }

        public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return null!;
            }

            return _dtoMapper.MapToUserProfileDto(user);
        }
        public async Task<bool> UpdateUserProfileAsync(UpdateUserProfileDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (user == null)
            {
                return false;
            }
            if (dto.FullName != null) user.FullName = dto.FullName;
            if (dto.PhoneNumber != null) user.PhoneNumber = dto.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;

        }
    }
}
