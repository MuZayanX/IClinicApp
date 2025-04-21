using IClinicApp.API.Dtos.Governorate;

namespace IClinicApp.API.Dtos.City
{
    public class CityDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cairo", "Giza"
        
        public GovernorateDto Governorate { get; set; } = null!; // e.g., "Cairo", "Giza"
    }
}
