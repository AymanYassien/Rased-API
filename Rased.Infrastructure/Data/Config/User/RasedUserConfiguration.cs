using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Models.SharedWallets;
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
                   .HasColumnType("NVARCHAR(255)");

            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(MAX)");

            builder.Property(x => x.DateOfBirth)
                   .HasColumnType("DATE");
                   
            builder.Property(x => x.ProfilePic)
                   .HasColumnType("IMAGE");
                   
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2");
                  

            // Relationships
            builder.HasMany(x => x.Wallets)
                   .WithOne(x => x.Creator)
                   .HasForeignKey(x => x.CreatorId)
                   .IsRequired();
            // M User => N SW
            builder.HasMany(x => x.SharedWallets)
                   .WithMany(x => x.Members)
                   .UsingEntity<SharedWalletMembers>(
                        l => l.HasOne(c => c.SharedWallet).WithMany(c => c.SWMembers).HasForeignKey(c => c.SharedWalletId),
                        r => r.HasOne(c => c.Member).WithMany(c => c.SWMembers).HasForeignKey(c => c.UserId)
                   );
        }
    }
}
