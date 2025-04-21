namespace IClinicApp.API.Dtos.Reviews
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public int Stars { get; set; } // Number of stars: 1-5
        public DateTime CreatedAt { get; set; }
    }
}
