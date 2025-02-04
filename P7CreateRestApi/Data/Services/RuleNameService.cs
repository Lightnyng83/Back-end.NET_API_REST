using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class RuleNameService : IRuleNameService
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task AddAsync(RuleName ruleName)
        {
            await _ruleNameRepository.AddAsync(ruleName);
        }

        public async Task<List<RuleName>> FindAllAsync()
        {
            return await _ruleNameRepository.FindAll();
        }

        public async Task DeleteAsync(int id)
        {
            await _ruleNameRepository.DeleteAsync(id);
        }

        public async Task<RuleName> FindByIdAsync(int id)
        {
            return await _ruleNameRepository.FindByIdAsync(id);
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            await _ruleNameRepository.UpdateAsync(ruleName);
        }
    }
}
