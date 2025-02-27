using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticWalletStatusDataConfiguration: IEntityTypeConfiguration<StaticWalletStatusData>
{
    public void Configure(EntityTypeBuilder<StaticWalletStatusData> entity)
    {
        entity.HasKey(e => e.id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("nvarchar(20)");

        entity.HasData(
            new StaticWalletStatusData { id = 1, Name = "ACTIVE" },
            new StaticWalletStatusData { id = 2, Name = "ARCHIVED" },
            new StaticWalletStatusData { id = 3, Name = "DELETED" }
            );
    }
}