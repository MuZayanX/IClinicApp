using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Dtos.Filtering
{
    public class DoctorFilterDto
    {
        public Guid? Id { get; set; }
        public SpecializationDto? Specialization { get; set; }
        public double? Rating { get; set; }
    }
}
