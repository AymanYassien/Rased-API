using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticFriendRequestStatusDataConfiguration : IEntityTypeConfiguration<StaticFriendRequestStatusData>
{
    public void Configure(EntityTypeBuilder<StaticFriendRequestStatusData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 

        /*entity.HasData(
            new StaticFriendRequestStatusData
            {
                Id = 1,
                Name = "PENDING"
            },
            new StaticFriendRequestStatusData
            {
                Id = 2,
                Name = "ACCEPTED"
            },
            new StaticFriendRequestStatusData
            {
                Id = 3,
                Name = "REJECTED"
            }
        );*/
    }
}