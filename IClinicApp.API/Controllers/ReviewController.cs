using IClinicApp.API.Dtos.Reviews;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IClinicApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        private readonly IReviewService _reviewService = reviewService;

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReview([FromBody] AddReviewDto addReviewDto)
        {
            if (addReviewDto == null)
            {
                return BadRequest("Review data is required.");
            }
            try
            {
                var review = await _reviewService.CreateReviewAsync(addReviewDto, User);
                return CreatedAtAction(nameof(GetReviewById), new { id = review.Id }, review);
            }
            catch (KeyNotFoundException knfEx)
            {
                return NotFound(knfEx.Message);
            }
            catch (UnauthorizedAccessException uaeEx)
            {
                return Forbid(uaeEx.Message);
            }
            catch (InvalidOperationException ioeEx)
            {
                return BadRequest(ioeEx.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while creating the review.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(Guid id)
        {
            var review = await _reviewService.GetReviewByIdAsync(id);
            if (review == null)
            {
                return NotFound($"Review is not found.");
            }

            return Ok(review);
        }

        [HttpGet("doctor/{doctorId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDoctorReviews(Guid doctorId)
        {
            try
            {
                var reviews = await _reviewService.GetReviewsForDoctorAsync(doctorId);
                return Ok(reviews);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the reviews.", details = ex.Message });
            }
        }
    }
}
