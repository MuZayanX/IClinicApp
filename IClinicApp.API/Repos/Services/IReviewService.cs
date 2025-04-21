using IClinicApp.API.Dtos.Reviews;

namespace IClinicApp.API.Repos.Services
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(AddReviewDto dto);
        Task<IEnumerable<ReviewDto>> GetReviewsForDoctorAsync(Guid doctorId);
    }
}
