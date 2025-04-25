using IClinicApp.API.Dtos.City;

namespace IClinicApp.API.Repos.Services
{
    public interface ICityService
    {
        Task<IEnumerable<CityDto>> GetCitiesByGovernorateId(Guid GovernorateId);
        Task<CityDto> GetCityByIdAsync(Guid id);
        Task<IEnumerable<CityDto>> GetAllCities();
    }
}
