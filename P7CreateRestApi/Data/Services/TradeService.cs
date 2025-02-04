using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<List<Trade>> GetTradesAsync()
        {
            return await _tradeRepository.GetAllAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.FindByIdAsync(id);
        }
        public async Task AddTradeAsync(Trade trade)
        {
            await _tradeRepository.AddAsync(trade);
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            await _tradeRepository.UpdateAsync(trade);
        }

        public async Task DeleteTradeAsync(int id)
        {
            await _tradeRepository.DeleteAsync(id);
        }
    }
}
