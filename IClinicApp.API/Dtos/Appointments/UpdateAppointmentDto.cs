using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Appointments
{
    public class UpdateAppointmentDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
    }
}
