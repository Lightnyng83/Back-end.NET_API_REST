using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Data.Repositories
{
    public class BidListRepository : IBidListRepository
    {
        private readonly LocalDbContext _dbContext;

        public BidListRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(BidList bidList)
        {
            await _dbContext.BidLists.AddAsync(bidList);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BidList>> FindAll()
        {
            return await _dbContext.BidLists.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await _dbContext.BidLists.FirstOrDefaultAsync(x => x.BidListId == id);
            if (del != null)
            {
                _dbContext.Remove(del);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<BidList> FindByIdAsync(int id)
        {
            return await _dbContext.BidLists.FirstOrDefaultAsync(x => x.BidListId == id);
        }

        public async Task Update(BidList bidList)
        {
            _dbContext.BidLists.Update(bidList);
            await _dbContext.SaveChangesAsync();
        }
    }
}
