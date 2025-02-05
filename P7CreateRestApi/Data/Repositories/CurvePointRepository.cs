using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Data.Repositories
{


    public class CurvePointRepository : ICurvePointRepository
    {
        private readonly LocalDbContext _dbContext;

        public CurvePointRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            await _dbContext.CurvePoints.AddAsync(curvePoint);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<CurvePoint> FindByIdAsync(int id)
        {
            return await _dbContext.CurvePoints.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<CurvePoint>> FindAll()
        {
            return await _dbContext.CurvePoints.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await _dbContext.CurvePoints.FirstOrDefaultAsync(x => x.Id == id);
            if (del != null)
            {
                _dbContext.Remove(del);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task Update(CurvePoint curvePoint)
        {
            _dbContext.CurvePoints.Update(curvePoint);
            await _dbContext.SaveChangesAsync();
        }
    }
}
