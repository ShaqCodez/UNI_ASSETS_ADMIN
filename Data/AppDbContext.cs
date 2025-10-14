using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using UNI_ASSETS.Models;

namespace UNI_ASSETS.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options)
        {
                
        }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AppUser> Staff { get; set; }
        public DbSet<Submission> Submissions { get; set; }
    }
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> opts):base(opts)
        {
                
        }
    }
}
