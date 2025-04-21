using IClinicApp.API.Dtos.Reviews;

namespace IClinicApp.API.Dtos.Doctors
{
    public class DoctorProfileDto : DoctorDto
    {
        public DoctorDto DoctorDto { get; set; } = null!;
        public string? Bio { get; set; } = string.Empty;
        public decimal? Price { get; set; } // e.g., 100.50 for 100.50 EGP
        public string ClinicAddress { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public int? ExperienceYears { get; set; }
        public List<ReviewDto> Reviews { get; set; } = [];
    }
}
