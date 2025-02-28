using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business.IncomesConfigures;

public class StaticIncomeSourceTypeDataConfiguration : IEntityTypeConfiguration<StaticIncomeSourceTypeData>
{
    public void Configure(EntityTypeBuilder<StaticIncomeSourceTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20);

        /*entity.HasData(
            new StaticIncomeSourceTypeData
            {
                Id = 1,
                Name = "SALARY"
            },
            new StaticIncomeSourceTypeData
            {
                Id = 2,
                Name = "FREELANCE"
            },
            new StaticIncomeSourceTypeData
            {
                Id = 3,
                Name = "INVESTMENT"
            },
            new StaticIncomeSourceTypeData
            {
                Id = 4,
                Name = "RENTAL"
            },
            new StaticIncomeSourceTypeData
            {
                Id = 5,
                Name = "BUSINESS"
            },
            new StaticIncomeSourceTypeData
            {
                Id = 6,
                Name = "OTHER"
            }
        );*/
    }
}