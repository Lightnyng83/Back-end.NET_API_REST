using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task AddAsync(Rating rating)
        {
            await _ratingRepository.AddAsync(rating);
        }

        public async Task<List<Rating>> FindAllAsync()
        {
            return await _ratingRepository.FindAll();
        }

        public async Task DeleteAsync(int id)
        {
            await _ratingRepository.DeleteAsync(id);
        }

        public async Task<Rating> FindByIdAsync(int id)
        {
            return await _ratingRepository.FindByIdAsync(id);
        }

        public async Task UpdateAsync(Rating rating)
        {
            await _ratingRepository.Update(rating);
        }
    }
}
