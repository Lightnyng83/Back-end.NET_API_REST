using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public async Task AddAsync(User user)
        {
            await _userRepository.AddAsync(user);
        }
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task UpdateAsync(User user)
        {
            await _userRepository.UpdateAsync(user);
        }
        public async Task<User?> FindByIdAsync(int id)
        {
            return await _userRepository.FindByIdAsync(id);
        }

        public async Task DeleteAsync(int id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
