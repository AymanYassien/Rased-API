using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticTriggerTypeDataConfiguration : IEntityTypeConfiguration<StaticTriggerTypeData>
{
    public void Configure(EntityTypeBuilder<StaticTriggerTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");;

        entity.HasData(
            new StaticTriggerTypeData
            {
                Id = 1,
                Name = "MONTHLY_ON_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 2,
                Name = "WEEKLY_ON_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 3,
                Name = "MONTHLY_ON_LAST_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 4,
                Name = "MONTHLY_ON_FIRST_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 5,
                Name = "WEEKLY_ON_LAST_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 6,
                Name = "WEEKLY_ON_FIRST_DAY"
            },
            new StaticTriggerTypeData
            {
                Id = 7,
                Name = "CUSTOM_INTERVAL"
            }
        );
    }
}