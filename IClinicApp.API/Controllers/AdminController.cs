using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController(IAdminService adminService) : ControllerBase
    {
        private readonly IAdminService _adminService = adminService;

        [HttpPost("doctor")]
        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorDto addDoctorDto)
        {
            if (addDoctorDto == null)
            {
                return BadRequest("Invalid doctor data.");
            }
            var doctor = await _adminService.AddDoctorAsync(addDoctorDto);
            return Ok(doctor );
        }

        [HttpPost("Governorate")]
        public async Task<IActionResult> AddGovernorate([FromBody] AddGovernorateDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new {message  =  "Invalid governorate data."});
            }
            var governorate = await _adminService.AddGovernorateAsync(dto);
            return Ok(governorate);
        }
        [HttpPost("City")]
        public async Task<IActionResult> AddCity([FromBody] AddCityDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Invalid city data." });
            }
            await _adminService.AddCityAsync(dto);
            return Ok(new {message ="City Added Successfully"  ,data = dto } );
        }

        [HttpPost("Specialization")]
        public async Task<IActionResult> AddSpecialization([FromBody] AddSpecializationDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new { message = "Invalid specialization data." });
            }
            await _adminService.AddSpecializationAsync(dto);
            return Ok(new {message = "Specialization is added successfully" , data =dto});
        }

    }
}
