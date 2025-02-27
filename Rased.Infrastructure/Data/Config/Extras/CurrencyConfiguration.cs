using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Extras;

namespace Rased.Infrastructure.Data.Config.Extras
{
    public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder)
        {
            builder.ToTable("Currencies");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Symbol)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasMany(x => x.Wallets)
                   .WithOne(x => x.Currency)
                   .HasForeignKey(x => x.CurrencyId)
                   .IsRequired();
            builder.HasMany(x => x.SharedWallets)
                   .WithOne(x => x.Currency)
                   .HasForeignKey(x => x.CurrencyId)
                   .IsRequired();
        }
    }
}
