using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Repos.Services
{
    public interface ISpecializationService
    {
        Task<IEnumerable<SpecializationDto>> GetAllSpecializationsAsync();
        Task<SpecializationDto> GetSpecializationByIdAsync(Guid id);
    }
}
