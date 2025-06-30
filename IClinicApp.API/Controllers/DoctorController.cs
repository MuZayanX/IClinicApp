using IClinicApp.API.Dtos.Filtering;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController(IDoctorService doctorService) : ControllerBase
    {
        private readonly IDoctorService _doctorService = doctorService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByIdAsync(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null)
            {
                return NotFound(new { message = "This Doctor is not found", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            return Ok(new { message = "Doctor found successfully", data = doctor, status = true, statusCode = StatusCodes.Status200OK });
        }

        [HttpGet("C/{cityId}")]
        public async Task<IActionResult> GetDoctorsByCityIdAsync(Guid cityId)
        {
            var doctors = await _doctorService.GetDoctorsByCityIdAsync(cityId);
            if (doctors == null || !doctors.Any())
            {
                return NotFound(new { message = "No doctors found for this city", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            var count = doctors.Count();
            return Ok(new { message = $"{count} Doctors were found", data = doctors, status = true, statusCode = StatusCodes.Status200OK });
        }

        [HttpGet("Recommended")]
        public async Task<IActionResult> GetRecommendedDoctorsAsync()
        {
            var recommendedDoctors = await _doctorService.GetRecommendedDoctorsAsync();
            if (recommendedDoctors == null || !recommendedDoctors.Any())
            {
                return NotFound(new { message = "No recommended doctors found", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            var count = recommendedDoctors.Count();
            return Ok(new { message = $"{count} Recommended Doctors were found", data = recommendedDoctors, status = true, statusCode = StatusCodes.Status200OK });
        }

        [HttpPost("Filter")]
        public async Task<IActionResult> GetDoctorsByFilterAsync([FromBody] DoctorFilterDto filter)
        {
            if (filter == null)
            {
                return BadRequest(new { message = "Invalid filter data", status = false, statusCode = StatusCodes.Status400BadRequest });
            }
            var doctors = await _doctorService.GetDoctorsByFilterAsync(filter);
            if (doctors == null || !doctors.Any())
            {
                return NotFound(new { message = "No doctors found for the given filter", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            var count = doctors.Count();
            return Ok(new { message = $"{count} Doctors were found", data = doctors, status = true, statusCode = StatusCodes.Status200OK });
        }
    }
}
