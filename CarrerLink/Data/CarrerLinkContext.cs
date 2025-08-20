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

        // Correct DbSets for your actual tables
        public DbSet<User> Users { get; set; } = default!;
        public DbSet<Applicant> Applicants { get; set; } = default!;
        public DbSet<Recruiter> Recruiters { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Optional: map to singular table names
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Applicant>().ToTable("Applicant");
            modelBuilder.Entity<Recruiter>().ToTable("Recruiter");
        }
    }
}
