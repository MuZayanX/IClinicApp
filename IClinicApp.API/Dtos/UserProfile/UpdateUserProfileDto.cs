namespace IClinicApp.API.Dtos.UserProfile
{
    public class UpdateUserProfileDto
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
