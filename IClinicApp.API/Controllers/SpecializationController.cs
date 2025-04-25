using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController(ISpecializationService specializationService) : ControllerBase
    {
        public readonly ISpecializationService _specializationService = specializationService;

        [HttpGet]
        public async Task<IActionResult> GetAllSpecializations()
        {
            var specializations = await _specializationService.GetAllSpecializationsAsync();
            int count = specializations.Count();
            return Ok(new { message = $"{count} Records have been fetched", data = specializations });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSpecializationById(Guid id)
        {
            var specialization = await _specializationService.GetSpecializationByIdAsync(id);
            if (specialization == null)
            {
                return NotFound(new { message = "Specialization not found" });
            }
            return Ok(new { message = "Specialization fetched successfully", data = specialization });
        }
    }
}
