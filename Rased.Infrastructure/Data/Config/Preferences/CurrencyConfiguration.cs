using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Preferences;

namespace Rased.Infrastructure.Data.Config.Preferences
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
        }
    }
}
