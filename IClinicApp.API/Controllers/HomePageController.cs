using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class HomePageController : ControllerBase
    {
        private readonly IHomePageService _homePageService;
        public HomePageController(IHomePageService homePageService)
        {
            _homePageService = homePageService;
                 
        }

        [HttpGet]
        public async Task<IActionResult> GetHomePage()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("User ID not found in claims.");
            }
            var homePageData = await _homePageService.GetHomePageAsync(Guid.Parse(userId));
            return Ok(homePageData);
        }
    }
}
