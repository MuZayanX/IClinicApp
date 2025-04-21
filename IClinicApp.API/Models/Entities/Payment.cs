using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IClinicApp.API.Models.Enums;

namespace IClinicApp.API.Models.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod Method { get; set; } // Manual, Card, Wallet
        public bool IsConfirmed { get; set; } 
        public DateTime PaymentDate { get; set; }

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;
    }
}
