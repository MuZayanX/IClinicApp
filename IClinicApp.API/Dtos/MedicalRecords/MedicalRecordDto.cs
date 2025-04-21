using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Dtos.MedicalRecords
{
    public class MedicalRecordDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;      // e.g., "Blood Test"
        public string? FileUrl { get; set; }                  // PDF or Image URL
        public DateTime RecordDate { get; set; }

        public Guid UserId { get; set; }
    }
}
