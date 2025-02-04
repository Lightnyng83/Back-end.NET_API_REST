using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Services
{
    public interface IUserService
    {
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task<User?> FindByIdAsync(int id);
        Task DeleteAsync(int id);
    }
}
