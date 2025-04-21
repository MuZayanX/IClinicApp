using IClinicApp.API.Dtos.Appointments;

namespace IClinicApp.API.Repos.Services
{
    public interface IAppointmentService
    {
        Task<bool> BookAppointmentAsync(BookAppointmentDto dto);
        Task<AppointmentDetailsDto> GetAppointmentDetailsAsync(Guid appointmentId);
    }
}
