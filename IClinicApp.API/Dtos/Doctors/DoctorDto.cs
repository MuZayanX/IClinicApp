using IClinicApp.API.Dtos.City;
using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Dtos.Doctors
{
    public class DoctorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public Guid SpecializationId { get; set; }
        public string SpecializationName { get; set; } = string.Empty;
        public string ClinicName { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public Guid CityId { get; set; }
        public string CityName { get; set; } =string.Empty;
        public string GovernorateName { get; set; } =string.Empty;
    }
}
