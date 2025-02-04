using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Data.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _dbContext;
        public RuleNameRepository(LocalDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task AddAsync(RuleName ruleName)
        {
            await _dbContext.RuleNames.AddAsync(ruleName);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<RuleName>> FindAll()
        {
            return await _dbContext.RuleNames.ToListAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await _dbContext.RuleNames.FirstOrDefaultAsync(x => x.Id == id);
            if (del != null)
            {
                _dbContext.Remove(del);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<RuleName> FindByIdAsync(int id)
        {
            return await _dbContext.RuleNames.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            _dbContext.RuleNames.Update(ruleName);
            await _dbContext.SaveChangesAsync();
        }
    }
}
