using LendingApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LendingApp.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Borrower> Borrower { get; set; }

        public DbSet<LoanDetails> LoanDetails { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Blacklist> Blacklist { get; set; }
        public DbSet<Quote> Quote { get; set; }
        public DbSet<LoanApplication> LoanApplication { get; set; }

    }
}
