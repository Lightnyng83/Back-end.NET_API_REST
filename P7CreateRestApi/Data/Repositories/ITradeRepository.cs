using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface ITradeRepository
    {
        Task AddAsync(Trade trade);
        Task<List<Trade>> GetAllAsync();
        Task DeleteAsync(int id);
        Task<Trade> FindByIdAsync(int id);
        Task UpdateAsync(Trade trade);
    }
}
