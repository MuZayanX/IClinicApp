using IClinicApp.API.Dtos.Payments;

namespace IClinicApp.API.Dtos.Appointments
{
    public class BookAppointmentDto
    {
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
        public Guid UserId { get; set; }
    }
}
