using Microsoft.EntityFrameworkCore;
using XMCrypto.Domain.Entities;
using XMCrypto.Persistance.Configuration;

namespace XMCrypto.Persistance
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<BitCoinPrice> BitCoinPrices { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BitCoinPriceConfiguration());
        }

    }
}
