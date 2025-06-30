using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Payments
{
    public class AddPaymentDto
    {
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // wallet , instapay, etc.
    }
}
