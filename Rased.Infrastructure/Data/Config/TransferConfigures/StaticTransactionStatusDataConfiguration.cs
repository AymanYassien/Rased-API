using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticTransactionStatusDataConfiguration : IEntityTypeConfiguration<StaticTransactionStatusData>
{
    public void Configure(EntityTypeBuilder<StaticTransactionStatusData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100); 

       
    }
}