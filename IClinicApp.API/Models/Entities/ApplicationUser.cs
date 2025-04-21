using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IClinicApp.API.Models.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; } = [];
        public ICollection<MedicalRecord> MedicalRecords { get; set; } = [];
        public ICollection<Notification> Notifications { get; set; } = [];
    }
}
