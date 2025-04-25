using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Governorate;

namespace IClinicApp.API.Dtos.City
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cairo", "Giza"
        public Guid GovernorateId { get; set; } // Foreign key to the Governorate
        public IEnumerable<DoctorDto> Doctors { get; set; } = []; // List of doctors in this city
    }
}
