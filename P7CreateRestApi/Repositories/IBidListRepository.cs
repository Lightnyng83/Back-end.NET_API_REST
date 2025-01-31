using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Repositories
{
    public interface IBidListRepository
    {
        void Add(BidList bidList);
        Task<List<BidList>> FindAll();
        void Delete(int id);
        BidList FindById(int id);
        void Update(BidList bidList);
    }
}
