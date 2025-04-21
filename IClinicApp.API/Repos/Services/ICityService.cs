namespace IClinicApp.API.Repos.Services
{
    public interface ICityService
    {
        Task<IEnumerable<string>> GetCitiesByGovernorateIdAsync(Guid governorateId);
    }
}
