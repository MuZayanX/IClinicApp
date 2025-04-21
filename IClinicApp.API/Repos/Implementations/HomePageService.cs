using IClinicApp.API.Data;
using IClinicApp.API.Dtos.HomePage;
using IClinicApp.API.Repos.Services;
using Microsoft.EntityFrameworkCore;

namespace IClinicApp.API.Repos.Implementations
{
    public class HomePageService : IHomePageService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDtoMapper _dtoMapper;
        public HomePageService(ApplicationDbContext dbContext, IDtoMapper dtoMapper)
        {
            _dbContext = dbContext;
            _dtoMapper = dtoMapper;
        }
        public async Task<HomePageDto> GetHomePageAsync(Guid userId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var specializations = await _dbContext.Specializations.ToListAsync();
            var recommendedDoctors = await _dbContext.Doctors
                .Include(d => d.Reviews)
                .Include(d => d.Specialization)
                .Include(d => d.City).ThenInclude(c => c.Governorate)
                .OrderByDescending(d => d.Reviews.Average(r => r.Rating))
                .Take(5)
                .ToListAsync();

            return _dtoMapper.MapToHomePageDto(user.FullName, recommendedDoctors, specializations);
        }
    }
}
