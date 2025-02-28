using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticReceiverTypeDataConfiguration : IEntityTypeConfiguration<StaticReceiverTypeData>
{
    public void Configure(EntityTypeBuilder<StaticReceiverTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 
        
        /*entity.HasData(
            new StaticReceiverTypeData
            {
                Id = 1,
                Name = "OWN_WALLET"
            },
            new StaticReceiverTypeData
            {
                Id = 2,
                Name = "APP_USER_NON_FRIEND"
            },
            new StaticReceiverTypeData
            {
                Id = 3,
                Name = "NON_APP_USER"
            },
            new StaticReceiverTypeData
            {
                Id = 4,
                Name = "FRIEND"
            },
            new StaticReceiverTypeData
            {
                Id = 5,
                Name = "SHARED_WALLET"
            }
        );*/
    }
}