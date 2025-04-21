using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IClinicApp.API.Models.Entities
{
    public class MedicalRecord
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;      // e.g., "Blood Test"
        public string? FileUrl { get; set; }                  // PDF or Image URL
        public DateTime RecordDate { get; set; }

        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
    }
}
