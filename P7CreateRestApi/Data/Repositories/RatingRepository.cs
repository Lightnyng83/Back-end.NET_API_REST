using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Data;
using Microsoft.EntityFrameworkCore;


namespace P7CreateRestApi.Data.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _context;

        public RatingRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Rating rating)
        {
            await _context.Ratings.AddAsync(rating);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Rating>> FindAll()
        {
            return await _context.Ratings.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id);
            if (del != null)
            {
                _context.Remove(del);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Rating> FindByIdAsync(int id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Update(Rating rating)
        {
            _context.Ratings.Update(rating);
            await _context.SaveChangesAsync();
        }
    }
}
