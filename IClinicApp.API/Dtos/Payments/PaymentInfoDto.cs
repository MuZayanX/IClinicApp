using IClinicApp.API.Dtos.Appointments;
using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Dtos.Payments
{
    public class PaymentInfoDto
    {
        public Guid Id { get; set; } // Payment ID
        public bool IsConfirmed { get; set; }
        public decimal Amount { get; set; }
        public Guid AppointmentId { get; set; } // Appointment ID
        public PaymentMethod Method { get; set; } // InstaPay, Wallet
        public DateTime PaymentDate { get; set; }

    }
}
