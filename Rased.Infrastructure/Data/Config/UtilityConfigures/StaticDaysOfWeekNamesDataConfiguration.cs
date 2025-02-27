using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticDaysOfWeekNamesDataConfiguration : IEntityTypeConfiguration<StaticDaysOfWeekNamesData>
{
    public void Configure(EntityTypeBuilder<StaticDaysOfWeekNamesData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 

        entity.HasData(
            new StaticDaysOfWeekNamesData
            {
                Id = 1,
                Name = "SATURDAY"
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 2,
                Name = "SUNDAY"
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 3,
                Name = "MONDAY"
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 4,
                Name = "TUESDAY"
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 5,
                Name = "WEDNESDAY"
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 6,
                Name = "THURSDAY" 
            },
            new StaticDaysOfWeekNamesData
            {
                Id = 7,
                Name = "FRIDAY"
            }
        );
    }
}