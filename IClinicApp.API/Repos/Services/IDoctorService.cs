using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Filtering;

namespace IClinicApp.API.Repos.Services
{
    public interface IDoctorService
    {
        Task<DoctorProfileDto> GetDoctorByIdAsync(Guid id);
        Task<IEnumerable<DoctorDto>> GetDoctorsByCityIdAsync(Guid cityId);
        Task<IEnumerable<DoctorDto>> GetRecommendedDoctorsAsync();
        Task<IEnumerable<DoctorDto>> GetDoctorsByFilterAsync(DoctorFilterDto filter);
    }
}
