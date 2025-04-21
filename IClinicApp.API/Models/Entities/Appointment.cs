using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using IClinicApp.API.Models.Enums;


namespace IClinicApp.API.Models.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public AppointmentStatus Status { get; set; } // e.g., Pending, Completed, Canceled

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;

        public Review? Review { get; set; } // optional
        public Payment? Payment { get; set; } // optional
    }
}
