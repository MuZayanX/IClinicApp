using IClinicApp.API.Dtos.Appointments;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
    {
        private readonly IAppointmentService _appointmentService = appointmentService;

        [HttpPost("book")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentDto bookAppointmentDto)
        {
            try
            {
                // the 'User' property from ControllerBase and represents the logged-in user's identity
                var appointmentDetails = await _appointmentService.BookAppointmentAsync(bookAppointmentDto, User);
                return Ok(new { message = "Appointment booked successfully", data = appointmentDetails });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Un Expected error occurred" });
            }
        }

        [HttpPost("{appointmentId}/confirm")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> ConfirmPayment(Guid appointmentId)
        {
            try
            {
                var updatedAppointment = await _appointmentService.ConfirmAppointmentPaymentAsync(appointmentId, User);
                return Ok(updatedAppointment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An Unexpected error occurred." });
            }
        }

        [HttpPost("{appointmentId}/complete")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CompleteAppointment(Guid appointmentId)
        {
            try
            {
                var completedAppointment = await _appointmentService.CompleteAppointmentAsync(appointmentId, User);
                return Ok(completedAppointment);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An Unexpected error occurred." });
            }
        }

        [HttpPost("{appointmentId}/cancel")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelAppointmentAsPatient(Guid appointmentId)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsPatientAsync(appointmentId, User);
                return Ok(new { message = "Appointment cancelled successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An Unexpected error occurred." });
            }
        }

        [HttpPost("{appointmentId}/doctor-cancel")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CancelAppointmentAsDoctor(Guid appointmentId)
        {
            try
            {
                await _appointmentService.CancelAppointmentAsDoctorAsync(appointmentId, User);
                return Ok(new { message = "Appointment cancelled successfully." });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An Unexpected error occurred." });
            }
        }
    }
}
