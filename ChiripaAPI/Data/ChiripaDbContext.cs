using ChiripaAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ChiripaAPI.Data
{
    public class ChiripaDbContext : IdentityDbContext<ApplicationUser>
    {
        public ChiripaDbContext(DbContextOptions<ChiripaDbContext> options) : base(options)
        {
            
        }

        public DbSet<Hielito> Hielitos { get; set; }
    }
}