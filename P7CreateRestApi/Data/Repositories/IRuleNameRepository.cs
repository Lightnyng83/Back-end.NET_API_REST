using Dot.Net.WebApi.Domain;

namespace P7CreateRestApi.Data.Repositories
{
    public interface IRuleNameRepository
    {
        Task AddAsync(RuleName ruleName);
        Task UpdateAsync(RuleName ruleName);
        Task DeleteAsync(int id);
        Task<List<RuleName>> FindAll();
        Task<RuleName> FindByIdAsync(int id);

    }
}
