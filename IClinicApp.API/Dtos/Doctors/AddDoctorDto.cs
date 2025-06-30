using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Reviews;
using IClinicApp.API.Dtos.Specialization;
using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Dtos.Doctors
{
    public class AddDoctorDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;

        public Guid SpecializationId { get; set; }

        public Guid CityId { get; set; }

        public string CllinicAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public int? ExperienceYears { get; set; }
        public decimal? Price { get; set; } // e.g., 100.50 for 100.50 EGP
    }
}
