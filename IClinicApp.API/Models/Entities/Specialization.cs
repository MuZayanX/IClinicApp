using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class Specialization
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cardiology", "Dermatology"

        public ICollection<Doctor> Doctors { get; set; } = [];
    }
}
