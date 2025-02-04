using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface IBidListRepository
    {
        Task AddAsync(BidList bidList);
        Task<List<BidList>> FindAll();
        Task DeleteAsync(int id);
        Task<BidList> FindByIdAsync(int id);
        Task Update(BidList bidList);


    }
}
