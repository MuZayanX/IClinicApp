using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Payments
{
    public class AddPaymentDto
    {
        public Guid AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // e.g., "Credit Card", "PayPal"
        public bool IsConfirmed { get; set; } = false; // Default to false, can be updated later
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
    }
}
