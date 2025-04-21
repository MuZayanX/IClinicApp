namespace IClinicApp.API.Dtos.Specialization
{
    public class SpecializationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cardiology", "Dermatology"
    }
}
