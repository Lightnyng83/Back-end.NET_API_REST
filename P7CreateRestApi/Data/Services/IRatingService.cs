using Dot.Net.WebApi.Controllers.Domain;

namespace P7CreateRestApi.Data.Services
{
    public interface IRatingService
    {
        Task AddAsync(Rating rating);
        Task<List<Rating>> FindAllAsync();
        Task DeleteAsync(int id);
        Task<Rating> FindByIdAsync(int id);
        Task UpdateAsync(Rating rating);
    }
}
