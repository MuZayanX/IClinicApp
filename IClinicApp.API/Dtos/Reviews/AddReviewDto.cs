namespace IClinicApp.API.Dtos.Reviews
{
    public class AddReviewDto
    {
        public Guid DoctorId { get; set; }
        public Guid AppointmentId { get; set; }
        public int Stars { get; set; }
        public string ReviewText { get; set; } = string.Empty;
    }
}
