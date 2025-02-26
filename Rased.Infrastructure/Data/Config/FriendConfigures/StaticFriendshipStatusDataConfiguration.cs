using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticFriendshipStatusDataConfiguration : IEntityTypeConfiguration<StaticFriendshipStatusData>
{
    public void Configure(EntityTypeBuilder<StaticFriendshipStatusData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 

        entity.HasData(
            new StaticFriendshipStatusData
            {
                Id = 1,
                Name = "ACTIVE"
            },
            new StaticFriendshipStatusData
            {
                Id = 2,
                Name = "BLOCKED_BY_USER1"
            },
            new StaticFriendshipStatusData
            {
                Id = 3,
                Name = "BLOCKED_BY_USER2"
            }
        );
    }
}