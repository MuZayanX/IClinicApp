using System.Security.Claims;
using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Reviews;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Models.Enums;
using IClinicApp.API.Repos.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class ReviewService(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IDtoMapper dtoMapper) : IReviewService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly IDtoMapper _dtoMapper = dtoMapper;

        public async Task<ReviewDto> CreateReviewAsync(AddReviewDto addReviewDto, ClaimsPrincipal user)
        {
            var userIdString = _userManager.GetUserId(user);
            if (string.IsNullOrEmpty(userIdString))
            {
                throw new InvalidOperationException("User is not authenticated.");
            }
            var userId = Guid.Parse(userIdString);

            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == addReviewDto.AppointmentId);

            if (appointment == null)
            {
                throw new KeyNotFoundException("Appointment not found.");
            }

            if (appointment.UserId != userId)
            {
                throw new UnauthorizedAccessException("You can only review your own appointments.");
            }

            if (appointment.Status != AppointmentStatus.Completed)
            {
                throw new InvalidOperationException("You can only review completed appointments.");
            }

            var reviewExists = await _context.Reviews
                .AnyAsync(r => r.AppointmentId == addReviewDto.AppointmentId);

            if (reviewExists)
            {
                throw new InvalidOperationException("You have already reviewed this appointment.");
            }

            var review = new Review
            {
                Id = Guid.NewGuid(),
                AppointmentId = addReviewDto.AppointmentId,
                DoctorId = appointment.DoctorId,
                Rating = addReviewDto.Stars,
                Comment = addReviewDto.ReviewText,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Reviews.AddAsync(review);
            await _context.SaveChangesAsync();

            var createdReview = await _context.Reviews
                .AsNoTracking()
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(r => r.Id == review.Id);

            if (createdReview == null)
            {
                throw new Exception("Failed to create review.");
            }

            return _dtoMapper.MapToReviewDto(createdReview);
        }

        public async Task<ReviewDto> GetReviewByIdAsync(Guid id)
        {
            var review = await _context.Reviews
                .AsNoTracking()
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (review == null)
            {
                return null!;
            }

            return _dtoMapper.MapToReviewDto(review);
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsForDoctorAsync(Guid doctorId)
        {
            var doctorExists = await _context.Doctors.AnyAsync(d => d.Id == doctorId);
            if (!doctorExists)
            {
                throw new KeyNotFoundException("Doctor not found.");
            }

            var reviews = await _context.Reviews
                .AsNoTracking()
                .Where(r => r.DoctorId == doctorId)
                .OrderByDescending(r => r.CreatedAt)
                .Include(r => r.Appointment)
                    .ThenInclude(a => a.User)
                .ToListAsync();

            return reviews.Select(r => _dtoMapper.MapToReviewDto(r));
        }
    }
}
