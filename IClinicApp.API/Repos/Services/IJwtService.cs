using IClinicApp.API.Models.Entities;

namespace IClinicApp.API.Repos.Services
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
