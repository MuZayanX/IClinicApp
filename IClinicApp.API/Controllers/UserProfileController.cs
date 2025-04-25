using System.Security.Claims;
using IClinicApp.API.Dtos.UserProfile;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;
        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            var profile = await _userProfileService.GetUserProfileAsync(userId);
            if (profile == null)
                return NotFound(new { Message = "User not found" });

            return Ok(profile);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> UpdateProfile(UpdateUserProfileDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized(new { Message = "Invalid or missing user ID in token." });

            // Inject the userId into the DTO so you don't rely on the client sending it
            dto.Id = userId;

            var result = await _userProfileService.UpdateUserProfileAsync(dto);

            if (!result)
                return BadRequest(new { Message = "Failed to update profile" });

            return Ok(new { Message = "Profile updated successfully" });
        }

    }
}
