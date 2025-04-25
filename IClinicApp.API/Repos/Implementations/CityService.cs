using IClinicApp.API.Data;
using IClinicApp.API.Dtos.City;
using IClinicApp.API.Repos.Services;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class CityService(ApplicationDbContext context, IDtoMapper dtoMapper) : ICityService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDtoMapper _dtoMapper = dtoMapper;

        public async Task<IEnumerable<CityDto>> GetCitiesByGovernorateId(Guid GovernorateId)
        {
            var cities = await _context.Cities
                .Where(c => c.GovernorateId == GovernorateId)
                .Include(c => c.Governorate) // Include the Governorate
                .Include(c => c.Doctors)
                .ThenInclude(d => d.Specialization)
                .ToListAsync();

            return cities.Select(c => _dtoMapper.MapToCityDto(c));
        }
        public async Task<CityDto> GetCityByIdAsync(Guid id)
        {
            var city = await _context.Cities
                .Include(c => c.Governorate) // Include the Governorate
                .Include(c => c.Doctors)
                .ThenInclude(d => d.Specialization)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return null!;
            }
            return _dtoMapper.MapToCityDto(city);
        }
        public async Task<IEnumerable<CityDto>> GetAllCities()
        {
            var cities = await _context.Cities
                .Include(c => c.Governorate) // Include the Governorate
                .Include(c => c.Doctors)
                .ThenInclude(d => d.Specialization)
                .OrderBy(c => c.Name) // Order by city name
                .ToListAsync();
            return cities.Select(c => _dtoMapper.MapToCityDto(c));
        }
    }
}
