using IClinicApp.API.Dtos.MedicalRecords;

namespace IClinicApp.API.Dtos.UserProfile
{
    public class UserProfileDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<MedicalRecordDto> MedicalRecords { get; set; } = new List<MedicalRecordDto>();
    }
}
