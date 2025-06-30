using IClinicApp.API.Dtos.Payments;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentController (IPaymentService paymentService) : ControllerBase
    {
        private readonly IPaymentService _paymentService = paymentService;
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreatePayment([FromBody] AddPaymentDto paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("Payment data is required.");
            }
            try
            {
                var paymentInfo = await _paymentService.CreatePaymentForAppointmentAsync(paymentDto, User);
                return Ok(paymentInfo);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
