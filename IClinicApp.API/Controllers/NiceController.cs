using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Authorize(Policy = "DoctorOnly")]
    [Route("api/[controller]")]
    [ApiController]
    public class NiceController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Nice");
        }
    }
}
