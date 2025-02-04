using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Services
{
    public interface ITradeService
    {
        Task AddTradeAsync(Trade trade);
        Task DeleteTradeAsync(int id);
        Task<Trade> GetTradeByIdAsync(int id);
        Task<List<Trade>> GetTradesAsync();
        Task UpdateTradeAsync(Trade trade);
    }
}
