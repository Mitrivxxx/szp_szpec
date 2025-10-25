using Microsoft.EntityFrameworkCore;
using MyWebApi.Model;

namespace MyWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> User { get; set; }
        public DbSet<Project> Project { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasMany(u => u.Projects)
                .WithMany(p => p.Users)
                .UsingEntity(j => j.ToTable("ProjectUsers"));
        }


    }
}