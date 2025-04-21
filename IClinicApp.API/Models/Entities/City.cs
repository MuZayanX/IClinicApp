using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Nasr City", "Assalam"

        public Guid GovernorateId { get; set; }
        public Governorate Governorate { get; set; } = null!;

        public ICollection<Doctor> Doctors { get; set; } = [];
    }
}
