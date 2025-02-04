using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface IUserRepository
    {
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task AddAsync(User user);
        Task<User?> FindByIdAsync(int id);
    }
}
