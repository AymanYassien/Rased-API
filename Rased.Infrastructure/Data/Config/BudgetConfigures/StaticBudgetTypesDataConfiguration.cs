using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;


public class StaticBudgetTypesDataConfiguration : IEntityTypeConfiguration<StaticBudgetTypesData>
{
    public void Configure(EntityTypeBuilder<StaticBudgetTypesData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasColumnType("nvarchar(20)");

        entity.HasData(
            new StaticBudgetTypesData
            {
                Id = 1,
                Name = "ACTIVE"
            },
            new StaticBudgetTypesData
            {
                Id = 2,
                Name = "EXCEEDED"
            },
            new StaticBudgetTypesData
            {
                Id = 3,
                Name = "WARNING"
            },
            new StaticBudgetTypesData
            {
                Id = 4,
                Name = "COMPLETED"
            },
            new StaticBudgetTypesData
            {
                Id = 5,
                Name = "INACTIVE"
            }
        );
    }
}