using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using P7CreateRestApi.Data.Repositories;

namespace P7CreateRestApi.Data.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher<IdentityUser> _passwordHasher;

        
        public UserService(IUserRepository userRepository,IPasswordHasher<IdentityUser> passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }
        public async Task AddAsync(User user)
        {
            var newuser = new IdentityUser
            {
                UserName = user.Username,
            };
            newuser.PasswordHash = _passwordHasher.HashPassword(newuser, user.Password);
            await _userRepository.AddAsync(user);
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
