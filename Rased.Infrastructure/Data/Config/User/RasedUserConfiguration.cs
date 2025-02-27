using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Data.Config.User
{
    public class RasedUserConfiguration : IEntityTypeConfiguration<RasedUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RasedUser> builder)
        {
            builder.Property(x => x.FirstName)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.LastName)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Country)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.DateOfBirth)
                   .HasColumnType("DATE")
                   .IsRequired(false);
            builder.Property(x => x.ProfilePic)
                   .HasColumnType("IMAGE")
                   .IsRequired(false);
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasMany(x => x.Wallets)
                   .WithOne(x => x.Creator)
                   .HasForeignKey(x => x.CreatorId)
                   .IsRequired();
            builder.HasMany(x => x.SharedWallets)
                   .WithOne(x => x.Creator)
                   .HasForeignKey(x => x.CreatorId)
                   .IsRequired();
        }
    }
}
