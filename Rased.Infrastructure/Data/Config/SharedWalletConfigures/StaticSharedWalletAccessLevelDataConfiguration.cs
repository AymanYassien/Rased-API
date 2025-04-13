using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.SharedWallets;

namespace Rased.Business;


public class StaticSharedWalletAccessLevelDataConfiguration : IEntityTypeConfiguration<StaticSharedWalletAccessLevelData>
{
    public void Configure(EntityTypeBuilder<StaticSharedWalletAccessLevelData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 

        /*entity.HasData(
            new StaticSharedWalletAccessLevelData
            {
                Id = 1,
                Name = "VIEW_ONLY"
            },
            new StaticSharedWalletAccessLevelData
            {
                Id = 2,
                Name = "CONTRIBUTE"
            },
            new StaticSharedWalletAccessLevelData
            {
                Id = 3,
                Name = "FULL_ACCESS"
            }
        );*/
    }
}