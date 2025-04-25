namespace IClinicApp.API.Dtos.City
{
    public class AddCityDto
    {
        public string Name { get; set; } = string.Empty;
        public Guid GovernorateId { get; set; }
    }
}
