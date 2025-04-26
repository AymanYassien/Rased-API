using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Data.Config.User
{
    public class RasedUserConfiguration : IEntityTypeConfiguration<RasedUser>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RasedUser> builder)
        {
            builder.Property(x => x.FullName)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Country)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired(false);
            builder.Property(x => x.Address)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false);
            builder.Property(x => x.UserBadge)
                   .HasColumnType("NVARCHAR(255)")
                   .HasDefaultValue(UserBadgeConstants.NEW);
            builder.Property(x => x.DateOfBirth)
                   .HasColumnType("DATE")
                   .IsRequired(false);
            builder.Property(x => x.ProfilePic)
                   .HasColumnType("IMAGE")
                   .IsRequired(false);

            builder.Property(x => x.AccountStatus)
                   .HasColumnType("NVARCHAR(50)")
                   .HasDefaultValue(AccountStatusConstants.INACTIVE);
            builder.Property(x => x.OTP)
                   .HasColumnType("NVARCHAR(20)")
                   .IsRequired(false);
            builder.Property(x => x.OtpExpiryTime)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            builder.Property(x => x.IsBanned)
                   .HasColumnType("BIT")
                   .HasDefaultValue(0);
            builder.Property(x => x.BannedDuration)
                   .HasColumnType("NVARCHAR(20)")
                   .IsRequired(false);
            builder.Property(x => x.BanBrokeAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);
            builder.Property(x => x.BannedReason)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false);

            //builder.Property(x => x.RefreshToken)
            //       .HasColumnType("NVARCHAR(MAX)")
            //       .IsRequired(false);
            //builder.Property(x => x.RefreshTokenExpiryTime)
            //       .HasColumnType("DATETIME2")
            //       .IsRequired(false);

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
