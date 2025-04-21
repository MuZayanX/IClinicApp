using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class Review
    {
        public Guid Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; } 
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid AppointmentId { get; set; }
        public Appointment Appointment { get; set; } = null!;

        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; } = null!;
    }
}
