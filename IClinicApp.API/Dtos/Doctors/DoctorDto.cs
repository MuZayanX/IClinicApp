using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Dtos.Doctors
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public SpecializationDto Specialization { get; set; } = null!;
        public string ClinicName { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;

        public CityDto City { get; set; } = null!;
    }
}
