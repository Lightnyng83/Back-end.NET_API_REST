using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Data.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _dbContext;

        public TradeRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Trade trade)
        {
            await _dbContext.Trades.AddAsync(trade);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<Trade>> GetAllAsync()
        {
            return await _dbContext.Trades.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await _dbContext.Trades.FirstOrDefaultAsync(x => x.TradeId == id);
            if (del != null)
            {
                _dbContext.Remove(del);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Trade> FindByIdAsync(int id)
        {
            return await _dbContext.Trades.FirstOrDefaultAsync(x => x.TradeId == id);
        }

        public async Task UpdateAsync(Trade trade)
        {
            _dbContext.Trades.Update(trade);
            await _dbContext.SaveChangesAsync();
        }
    }
}
