using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Repos.Services
{
    public interface IAddDoctorService
    {
        Task<Doctor> AddDoctorAsync(AddDoctorDto addDoctorDto);
    }
}
