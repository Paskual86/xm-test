using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using XMCrypto.Domain.Entities;

namespace XMCrypto.Persistance.Configuration
{
    public class BitCoinPriceConfiguration : IEntityTypeConfiguration<BitCoinPrice>
    {
        public void Configure(EntityTypeBuilder<BitCoinPrice> builder)
        {
            builder.HasKey(key => key.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.SellPrice).IsRequired();
            builder.Property(p => p.BuyPrice).IsRequired();
            builder.Property(p => p.Source).HasMaxLength(200).IsRequired();
            builder.Property(p => p.StoreDateTime).IsRequired().HasColumnType("Datetime");
        }
    }
}
