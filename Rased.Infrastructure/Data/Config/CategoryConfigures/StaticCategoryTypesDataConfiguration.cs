using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticCategoryTypesDataConfiguration : IEntityTypeConfiguration<StaticCategoryTypesData>
{
    public void Configure(EntityTypeBuilder<StaticCategoryTypesData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50); 
    }
}