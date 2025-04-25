using IClinicApp.API.Dtos.City;

namespace IClinicApp.API.Dtos.Governorate
{
    public class GovernorateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cairo", "Giza"
        public IEnumerable<CityDto> Cities { get; set; } = []; // List of cities associated with this governorate
    }
}
