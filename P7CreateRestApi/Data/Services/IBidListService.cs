using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Services
{
    public interface IBidListService
    {
        Task AddAsync(BidList bidList);
        Task<List<BidList>> FindAllAsync();
        Task DeleteAsync(int id);
        Task<BidList> FindByIdAsync(int id);
        Task UpdateAsync(BidList bidList);
    }
}
