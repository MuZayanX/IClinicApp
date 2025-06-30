using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Filtering;
using IClinicApp.API.Repos.Services;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class DoctorService(ApplicationDbContext context, IDtoMapper dtoMapper) : IDoctorService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDtoMapper _dtoMapper = dtoMapper;

        public async Task<DoctorProfileDto> GetDoctorByIdAsync(Guid id)
        {
            var doctor = await _context.Doctors
                .Include(d => d.Specialization) // Include the Specialization
                .Include(d => d.City) // Include the City
                .ThenInclude(c => c.Governorate) // Include the Governorate of the City
                .Include(d => d.Reviews) // Include the Reviews
                .Include(d => d.Appointments) // Include the Appointments
                .FirstOrDefaultAsync(d => d.Id == id);
            if (doctor == null)
            {
                return null!;
            }
            return _dtoMapper.MapToDoctorProfileDto(doctor);
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsByCityIdAsync(Guid cityId)
        {
            var doctors = await _context.Doctors
                .Where(d => d.CityId == cityId)
                .Include(d => d.Specialization) // Include the Specialization
                .Include(d => d.City) // Include the City
                .ThenInclude(c => c.Governorate) // Include the Governorate of the City
                .OrderByDescending(d =>d.Reviews!.Count != 0 ? d.Reviews.Average(r => r.Rating) : 0)
                .ToListAsync();
            return doctors.Select(d => _dtoMapper.MapToDoctorDto(d));
        }

        public async Task<IEnumerable<DoctorDto>> GetRecommendedDoctorsAsync()
        {
            var recommendedDoctors = await _context.Doctors
                .Include(d => d.Specialization) // Include the Specialization
                .Include(d => d.City) // Include the City
                .ThenInclude(c => c.Governorate) // Include the Governorate of the City
                .OrderByDescending(d => d.Reviews!.Count != 0 ? d.Reviews.Average(r => r.Rating) : 0)
                .Take(10) // Get top 10 recommended doctors
                .ToListAsync();

            return recommendedDoctors.Select(d => _dtoMapper.MapToDoctorDto(d));
        }

        public async Task<IEnumerable<DoctorDto>> GetDoctorsByFilterAsync(DoctorFilterDto filter)
        {
            var doctors = _context.Doctors
                .Include(d => d.Specialization) // Include the Specialization
                .Include(d => d.City) // Include the City
                .ThenInclude(c => c.Governorate) // Include the Governorate of the City
                .AsQueryable();
            if (filter.CityId.HasValue)
            {
                doctors = doctors.Where(d => d.CityId == filter.CityId.Value);
            }
            if (filter.SpecializationId.HasValue)
            {
                doctors = doctors.Where(d => d.SpecializationId == filter.SpecializationId.Value);
            }
            if (filter.MaxExperienceYears >= 0 && filter.MinExperienceYears >=0)
            {
                doctors = doctors.Where(d => d.ExperienceYears >= filter.MinExperienceYears && d.ExperienceYears <= filter.MaxExperienceYears);
            }
            if (filter.MaxRating >= 0 && filter.MinRating >= 0)
            {
                doctors = doctors.Where(d => d.Reviews!.Count != 0 && d.Reviews.Average(r => r.Rating) >= filter.MinRating && d.Reviews.Average(r => r.Rating) <= filter.MaxRating);
            }
            if (filter.MinPrice.HasValue && filter.MaxPrice.HasValue)
            {
                doctors = doctors.Where(d => d.Price >= filter.MinPrice.Value && d.Price <= filter.MaxPrice.Value);
            }
            if (filter.GovernorateId.HasValue)
            {
                doctors = doctors.Where(d => d.City!.GovernorateId == filter.GovernorateId.Value);
            }
            if (!string.IsNullOrEmpty(filter.Name))
            {
                doctors = doctors.Where(d => d.FullName.ToUpper().Contains(filter.Name.ToUpper()));
            }
            var doctorList = await doctors
                .OrderByDescending(d => d.Reviews!.Count != 0 ? d.Reviews.Average(r => r.Rating) : 0)
                .ToListAsync();
            return doctorList.Select(d => _dtoMapper.MapToDoctorDto(d));
        }
    }
}

