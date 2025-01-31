using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Services
{
    public class BidListService : IBidListService
    {
        private readonly IBidListRepository _bidListRepository;
        public BidListService(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }
        public void Add(BidList bidList)
        {
            _bidListRepository.Add(bidList);
            
        }

        public async Task<List<BidList>> FindAll()
        {
            return await _bidListRepository.FindAll();
        }

        public void Delete(int id)
        {
            _bidListRepository.Delete(id);
        }

        public BidList FindById(int id)
        {
            return _bidListRepository.FindById(id);
        }

        public void Update(BidList bidList)
        {
            _bidListRepository.Update(bidList);
        }
    }
}
