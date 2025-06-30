using System.Security.Claims;
using IClinicApp.API.Dtos.Payments;

namespace IClinicApp.API.Repos.Services
{
    public interface IPaymentService
    {
        Task<PaymentInfoDto> CreatePaymentForAppointmentAsync(AddPaymentDto paymentDto, ClaimsPrincipal user);
    }
}
