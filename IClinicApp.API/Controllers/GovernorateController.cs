using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GovernorateController(IGovernorateService governorateService) : ControllerBase
    {
        private readonly IGovernorateService _governorateService = governorateService;

        [HttpGet]
        public async Task<IActionResult> GetAllGovernorates()
        {
            var governorates = await _governorateService.GetAllGovernoratesAsync();
            int count = governorates.Count();
            return Ok(new { message = $"{count} Records have been fetched", data = governorates });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGovernorateById(Guid id)
        {
            var governorate = await _governorateService.GetGovernorateByIdAsync(id);
            if (governorate == null)
            {
                return NotFound(new { message = "Governorate not found" });
            }
            return Ok(new { message = "Governorate fetched successfully", data = governorate });
        }
    }
}
