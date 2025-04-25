using IClinicApp.API.Data;
using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;

namespace IClinicApp.API.Repos.Implementations
{
    public class AdminService(ApplicationDbContext context, IDtoMapper dtoMapper) : IAdminService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IDtoMapper _dtoMapper = dtoMapper;

        public async Task<Doctor> AddDoctorAsync(AddDoctorDto addDoctorDto)
        {
            var doctor = _dtoMapper.MapToAddDoctor(addDoctorDto);
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }
        public async Task<Governorate> AddGovernorateAsync(AddGovernorateDto dto)
        {
            var governorate = new Governorate
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await _context.Governorates.AddAsync(governorate);
            await _context.SaveChangesAsync();
            return governorate;
        }
        public async Task<City> AddCityAsync(AddCityDto dto)
        {
            var city = new City
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                GovernorateId = dto.GovernorateId,
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return city;
        }
        public async Task<Specialization> AddSpecializationAsync(AddSpecializationDto dto)
        {
            var specialization = new Specialization
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
            };
            await _context.Specializations.AddAsync(specialization);
            await _context.SaveChangesAsync();
            return specialization;
        }

    }
}
