using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Governorate;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Repos.Services
{
    public interface IAdminService
    {
        Task<Doctor> AddDoctorAsync(AddDoctorDto addDoctorDto);
        Task<Governorate> AddGovernorateAsync(AddGovernorateDto dto);
        Task<City> AddCityAsync(AddCityDto dto);
        Task<Specialization> AddSpecializationAsync(AddSpecializationDto dto);
    }
}
