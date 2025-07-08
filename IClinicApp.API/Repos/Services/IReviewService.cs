using System.Security.Claims;
using IClinicApp.API.Dtos.Reviews;

namespace IClinicApp.API.Repos.Services
{
    public interface IReviewService
    {
        Task<ReviewDto> CreateReviewAsync(AddReviewDto addReviewDto, ClaimsPrincipal user);
        Task<ReviewDto> GetReviewByIdAsync(Guid id);
        Task<IEnumerable<ReviewDto>> GetReviewsForDoctorAsync(Guid doctorId);
    }
}
