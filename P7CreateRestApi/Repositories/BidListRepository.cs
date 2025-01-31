using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Repositories
{
    public class BidListRepository : IBidListRepository
    {
        private readonly LocalDbContext _dbContext;

        public BidListRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(BidList bidList)
        {
            _dbContext.BidLists.Add(bidList);
            _dbContext.SaveChanges();
        }

        public async Task<List<BidList>> FindAll()
        {
            return await _dbContext.BidLists.ToListAsync();
        }

        public void Delete(int id)
        {
            var del = _dbContext.BidLists.FirstOrDefault(x => x.BidListId == id);
            if (del != null)
            {
                _dbContext.Remove(del);
                _dbContext.SaveChanges();
            }
        }

        public BidList FindById(int id)
        {
            return _dbContext.BidLists.FirstOrDefault(x => x.BidListId == id);
        }

        public void Update(BidList bidList)
        {
            _dbContext.BidLists.Update(bidList);
            _dbContext.SaveChanges();
        }
    }
}
