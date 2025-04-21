namespace IClinicApp.API.Repos.Services
{
    public interface ISpecializationService
    {
        Task<IEnumerable<string>> GetAllSpecializationsAsync();
    }
}
