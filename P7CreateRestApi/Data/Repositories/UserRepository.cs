using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public LocalDbContext DbContext { get; }

        public UserRepository(LocalDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<User?> FindByUserNameAsync(string userName)
        {
            return await DbContext.Users.Where(user => user.Username == userName)
                                  .FirstOrDefaultAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var del = await DbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (del != null)
            {
                DbContext.Remove(del);
                await DbContext.SaveChangesAsync();
            }

        }

        public async Task AddAsync(User user)
        {
            await DbContext.Users.AddAsync(user);
            await DbContext.SaveChangesAsync();
        }

        public async Task<User?> FindByIdAsync(int id)
        {
            return await DbContext.Users.Where(user => user.Id == id)
                                  .FirstOrDefaultAsync();
        }
    }
}