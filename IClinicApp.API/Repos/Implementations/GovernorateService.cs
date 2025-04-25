using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Repos.Services;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class GovernorateService(ApplicationDbContext dbContext, IDtoMapper dtoMapper) : IGovernorateService
    {
        private readonly ApplicationDbContext _context = dbContext;
        private readonly IDtoMapper _dtoMapper = dtoMapper;

        public async Task<IEnumerable<GovernorateDto>> GetAllGovernoratesAsync()
        {
            var governorates = await _context.Governorates
                .Include(g => g.Cities)
                .ThenInclude(c => c.Doctors)
                .ToListAsync();
            return governorates.Select(g => _dtoMapper.MapToGovernorateDto(g));
        }
        public async Task<GovernorateDto> GetGovernorateByIdAsync(Guid id)
        {
            var governorate = await _context.Governorates
                .Include(g => g.Cities)
                .ThenInclude(c => c.Doctors)
                .FirstOrDefaultAsync(g => g.Id == id);
            if (governorate == null)
            {
                return null!;
            }
            return _dtoMapper.MapToGovernorateDto(governorate);
        }
    }
}
