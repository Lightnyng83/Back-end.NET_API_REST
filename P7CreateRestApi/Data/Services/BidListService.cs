using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class BidListService : IBidListService
    {
        private readonly IBidListRepository _bidListRepository;
        public BidListService(IBidListRepository bidListRepository)
        {
            _bidListRepository = bidListRepository;
        }
        public async Task AddAsync(BidList bidList)
        {
            await _bidListRepository.AddAsync(bidList);

        }

        public async Task<List<BidList>> FindAllAsync()
        {
            return await _bidListRepository.FindAll();
        }

        public async Task DeleteAsync(int id)
        {
           await _bidListRepository.DeleteAsync(id);
        }

        public async Task<BidList> FindByIdAsync(int id)
        {
            return await _bidListRepository.FindByIdAsync(id);
        }

        public async Task UpdateAsync(BidList bidList)
        {
            await _bidListRepository.Update(bidList);
        }
    }
}
