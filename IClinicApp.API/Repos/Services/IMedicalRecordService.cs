using IClinicApp.API.Dtos.MedicalRecords;

namespace IClinicApp.API.Repos.Services
{
    public interface IMedicalRecordService
    {
        Task<IEnumerable<MedicalRecordDto>> GetMedicalRecordsAsync(Guid userId);
        Task<bool> AddMedicalRecordAsync(MedicalRecordDto dto);
    }
}
