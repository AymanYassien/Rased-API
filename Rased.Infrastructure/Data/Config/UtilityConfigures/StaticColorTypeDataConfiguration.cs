using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticColorTypeDataConfiguration:IEntityTypeConfiguration<StaticColorTypeData>
{
    public void Configure(EntityTypeBuilder<StaticColorTypeData> entity)
    {
        entity.HasKey(e => e.id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20)
            .HasColumnType("nvarchar(20)");

        /*entity.HasData(
            new StaticColorTypeData { id = 1, Name = "Black" },
            new StaticColorTypeData { id = 2, Name = "White" },
            new StaticColorTypeData { id = 3, Name = "Red" },
            new StaticColorTypeData { id = 4, Name = "Blue" },
            new StaticColorTypeData { id = 5, Name = "Green" },
            new StaticColorTypeData { id = 6, Name = "Yellow" }
        );*/
    }
    
}