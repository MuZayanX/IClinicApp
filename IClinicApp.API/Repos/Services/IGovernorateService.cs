namespace IClinicApp.API.Repos.Services
{
    public interface IGovernorateService
    {
        Task<IEnumerable<string>> GetAllGovernoratesAsync();
    }
}
