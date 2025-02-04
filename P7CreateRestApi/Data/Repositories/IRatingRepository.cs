using Dot.Net.WebApi.Controllers.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface IRatingRepository
    {
        Task AddAsync(Rating rating);
        Task<List<Rating>> FindAll();
        Task DeleteAsync(int id);
        Task<Rating> FindByIdAsync(int id);
        Task Update(Rating rating);
    }
}
