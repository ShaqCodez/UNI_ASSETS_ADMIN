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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           var analyticsTable= modelBuilder.Entity<AppAnalytics>();
            analyticsTable.HasKey(x => x.LogId);
            analyticsTable.HasMany(x => x.Submissions).WithOne(x=>x.Analysis);
            
        }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<AppUser> Staff { get; set; }
        public DbSet<Submission> Submissions { get; set; }
        public DbSet<AppAnalytics> Analytics { get; set; }
    }
    public class IdentityContext : IdentityDbContext<AppUser>
    {
        public IdentityContext(DbContextOptions<IdentityContext> opts):base(opts)
        {
                
        }
    }
}
