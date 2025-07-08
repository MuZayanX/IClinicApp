using System.Security.Claims;
using IClinicApp.API.Dtos.Appointments;
using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Repos.Services
{
    public interface IAppointmentService
    {
        Task<AppointmentDetailsDto> BookAppointmentAsync(BookAppointmentDto dto, ClaimsPrincipal user);
        Task<AppointmentDetailsDto> ConfirmAppointmentPaymentAsync(Guid appointmentId, ClaimsPrincipal doctorUser);
        Task<AppointmentDetailsDto> CompleteAppointmentAsync(Guid appointmentId, ClaimsPrincipal doctorUser);
        Task CancelAppointmentAsPatientAsync(Guid appointmentId, ClaimsPrincipal user);
        Task CancelAppointmentAsDoctorAsync(Guid appointmentId, ClaimsPrincipal doctorUser);
        Task<IEnumerable<AppointmentDetailsDto>> GetMyAppointmentsAsync(string status, ClaimsPrincipal user);
    }
}
