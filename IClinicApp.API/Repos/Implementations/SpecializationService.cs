using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class SpecializationService(ApplicationDbContext context, IDtoMapper dtoMapper) : ISpecializationService
    {
        private readonly IDtoMapper _dtoMapper = dtoMapper;
        private readonly ApplicationDbContext _context = context;
        
        public async Task<IEnumerable<SpecializationDto>> GetAllSpecializationsAsync()
        {
            var specializations = await _context.Specializations
                .Include(s => s.Doctors)
                .ThenInclude(d => d.City).ThenInclude(c => c.Governorate)
                .ToListAsync();
            return specializations.Select(s => _dtoMapper.MapToSpecializationDto(s));
    
        }
        public async Task<SpecializationDto> GetSpecializationByIdAsync(Guid id)
        {
            var specialization = await _context.Specializations
                .Include(s => s.Doctors)
                .ThenInclude(d => d.City).ThenInclude(c => c.Governorate)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (specialization == null)
            {
                return null!;
            }
            return _dtoMapper.MapToSpecializationDto(specialization);
        }
    }


}
