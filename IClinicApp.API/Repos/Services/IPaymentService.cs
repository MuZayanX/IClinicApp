using IClinicApp.API.Dtos.Payments;

namespace IClinicApp.API.Repos.Services
{
    public interface IPaymentService
    {
        Task<PaymentInfoDto> GetPaymentInfoAsync(Guid appointmentId);
        Task<bool> AddPaymentAsync(PaymentInfoDto dto);
    }
}
