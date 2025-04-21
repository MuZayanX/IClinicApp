using IClinicApp.API.Dtos.Doctors;
using IClinicApp.API.Dtos.Specialization;

namespace IClinicApp.API.Dtos.HomePage
{
    public class HomePageDto
    {
        public string FullName { get; set; } = string.Empty;
        public List<SpecializationDto> Specializations { get; set; } = [];
        public List<DoctorDto> RecommendedDoctors { get; set; } = [];
    }
}
