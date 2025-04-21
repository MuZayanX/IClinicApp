using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class Governorate
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Cairo", "Giza"

        public ICollection<City> Cities { get; set; } = [];
    }
}
