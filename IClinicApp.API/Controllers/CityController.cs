using IClinicApp.API.Dtos.City;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CityController(ICityService cityService) : ControllerBase
    {
        private readonly ICityService _cityService = cityService;

        [HttpGet("G/{GovernorateId}")]
        public async Task<IActionResult> GetCitiesByGovernorateId(Guid GovernorateId)
        {
            var cities = await _cityService.GetCitiesByGovernorateId(GovernorateId);
            if (cities == null || !cities.Any())
            {
                return NotFound(new { message = "No cities found for this governorate", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            var count = cities.Count();
            return Ok(new { message = $"{count} Cities were found", data = cities, status = true, statusCode = StatusCodes.Status200OK });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCityByIdAsync(Guid id)
        {
            var city = await _cityService.GetCityByIdAsync(id);
            if (city == null)
            {
                return NotFound(new {message = "This City is not found" , status =false , statusCode = StatusCodes.Status404NotFound});
            }
            return Ok(new {message = "City found successfully", data = city, status = true, statusCode = StatusCodes.Status200OK});
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCities()
        {
            var cities = await _cityService.GetAllCities();
            if (cities == null || !cities.Any())
            {
                return NotFound(new { message = "No cities found", status = false, statusCode = StatusCodes.Status404NotFound });
            }
            var count = cities.Count();
            return Ok(new { message = $"{count} Cities were found", data = cities, status = true, statusCode = StatusCodes.Status200OK });
        }
    }
}
