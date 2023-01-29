using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMCrypto.Domain.Entities;

namespace XMCrypto.Persistance.Configuration
{
    public class BitCoinPriceConfiguration : IEntityTypeConfiguration<BitCoinPrice>
    {
        public void Configure(EntityTypeBuilder<BitCoinPrice> builder)
        {
            builder.HasKey(key => key.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.Property(p => p.Price).IsRequired();
            builder.Property(p => p.Source).HasMaxLength(200).IsRequired();
            builder.Property(p => p.StoreDateTime).IsRequired().HasColumnType("Datetime");
        }
    }
}
