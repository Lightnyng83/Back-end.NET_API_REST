using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public interface IRuleNameService
    {

        Task AddAsync(RuleName ruleName);

        Task<List<RuleName>> FindAllAsync();
        Task DeleteAsync(int id);
        Task<RuleName> FindByIdAsync(int id);
        Task UpdateAsync(RuleName ruleName);


    }
}
