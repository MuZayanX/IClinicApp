namespace IClinicApp.API.Dtos.Governorate
{
    public class GovernorateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cairo", "Giza"
    }
}
