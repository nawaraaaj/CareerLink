using Microsoft.EntityFrameworkCore;
using CarrerLink.Models;

namespace CarrerLink.Data
{
    public class CarrerLinkContext : DbContext
    {
        public CarrerLinkContext(DbContextOptions<CarrerLinkContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; } = default!; // must match how you access it
        public DbSet<Applicant> Applicant { get; set; } = default!;
        public DbSet<Recruiter> Recruiter { get; set; } = default!;

        public DbSet<Job> Job { get; set; } = default!;
      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin user
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Name = "Admin",
                    Email = "admin@carrerlink.com",
                    Password = "admin123",
                    Mobile = "9800000000",
                    UserType = "Admin"
                }
            );
        }
    }
}
