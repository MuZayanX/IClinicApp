using IClinicApp.API.Dtos.HomePage;

namespace IClinicApp.API.Repos.Services
{
    public interface IHomePageService
    {
        Task<HomePageDto> GetHomePageAsync(Guid userId);
    }
}
