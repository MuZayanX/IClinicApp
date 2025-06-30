using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Dtos.Filtering
{
    public class DoctorFilterDto
    {
        public string? Name { get; set; } = string.Empty;
        public Guid? SpecializationId { get; set; }
        public Guid? CityId { get; set; }
        public Guid? GovernorateId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? MinExperienceYears { get; set; }
        public int? MaxExperienceYears { get; set; }
        public float? MinRating { get; set; }
        public float? MaxRating { get; set; }
    }
}
