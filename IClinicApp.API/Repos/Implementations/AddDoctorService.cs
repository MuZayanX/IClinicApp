using IClinicApp.API.Data;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Models.Entities;
using IClinicApp.API.Repos.Services;

namespace IClinicApp.API.Repos.Implementations
{
    public class AddDoctorService : IAddDoctorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDtoMapper _dtoMapper;
        public AddDoctorService(ApplicationDbContext context, IDtoMapper dtoMapper)
        {
            _context = context;
            _dtoMapper = dtoMapper;
        }
        public async Task<Doctor> AddDoctorAsync(AddDoctorDto addDoctorDto)
        {
            var doctor = _dtoMapper.MapToAddDoctor(addDoctorDto);
            await _context.Doctors.AddAsync(doctor);
            await _context.SaveChangesAsync();
            return doctor;
        }

    }
}
