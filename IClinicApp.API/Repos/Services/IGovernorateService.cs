using IClinicApp.API.Dtos.Governorate;

namespace IClinicApp.API.Repos.Services
{
    public interface IGovernorateService
    {
        Task<IEnumerable<GovernorateDto>> GetAllGovernoratesAsync();
        Task<GovernorateDto> GetGovernorateByIdAsync(Guid id);
    }
}
