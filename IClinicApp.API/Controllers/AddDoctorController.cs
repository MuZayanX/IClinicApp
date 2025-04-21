using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AddDoctorController : ControllerBase
    {
        private readonly IAddDoctorService _addDoctorService;
        public AddDoctorController(IAddDoctorService addDoctorService)
        {
            _addDoctorService = addDoctorService;
        }

        [HttpPost]
        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorDto addDoctorDto)
        {
            if (addDoctorDto == null)
            {
                return BadRequest("Invalid doctor data.");
            }
            var doctor = await _addDoctorService.AddDoctorAsync(addDoctorDto);
            return Ok(doctor );
        }

        
    }
}
