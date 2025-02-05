using Dot.Net.WebApi.Controllers.Domain;
using Dot.Net.WebApi.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace P7CreateRestApi.Test
{
    public class LocalDbTestContext : IdentityDbContext<IdentityUser>
    {
        public LocalDbTestContext(DbContextOptions<LocalDbTestContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Trade> Trades { get; set; }
        public DbSet<RuleName> RuleNames { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<BidList> BidLists { get; set; }
        public DbSet<CurvePoint> CurvePoints { get; set; }

    }
}
