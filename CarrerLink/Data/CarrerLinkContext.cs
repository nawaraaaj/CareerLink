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
    }
}
