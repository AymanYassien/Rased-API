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
            .HasMaxLength(20); 

        entity.HasData(
            new StaticTransactionStatusData
            {
                Id = 1,
                Name = "PENDING"
            },
            new StaticTransactionStatusData
            {
                Id = 2,
                Name = "ACCEPTED"
            },
            new StaticTransactionStatusData
            {
                Id = 3,
                Name = "REJECTED"
            },
            new StaticTransactionStatusData
            {
                Id = 4,
                Name = "COMPLETED"
            },
            new StaticTransactionStatusData
            {
                Id = 5,
                Name = "CANCELED"
            }
        );
    }
}