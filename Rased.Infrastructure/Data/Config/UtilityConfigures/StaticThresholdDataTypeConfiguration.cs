using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;


public class StaticThresholdTypeDataConfiguration : IEntityTypeConfiguration<StaticThresholdTypeData>
{
    public void Configure(EntityTypeBuilder<StaticThresholdTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        entity.HasData(
            new StaticThresholdTypeData
            {
                Id = 1,
                Name = "PERCENTAGE"
            },
            new StaticThresholdTypeData
            {
                Id = 2,
                Name = "FIXED_AMOUNT"
            }
        );
    }
}
