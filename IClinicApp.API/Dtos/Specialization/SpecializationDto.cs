using IClinicApp.API.Dtos.Doctors;

namespace IClinicApp.API.Dtos.Specialization
{
    public class SpecializationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cardiology", "Dermatology"
        public IEnumerable<DoctorDto> Doctors { get; set; } = []; // List of doctors associated with this specialization
    }
}
