using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.SharedWallets;


namespace Rased.Infrastructure.Data.Config.SharedWalletConfigures
{
    public class SharedWalletMembersConfiguration : IEntityTypeConfiguration<SharedWalletMembers>
    {
        public void Configure(EntityTypeBuilder<SharedWalletMembers> builder)
        {
            builder.HasKey(swm => swm.MembershipId);

            builder.Property(swm => swm.SharedWalletId)
                .IsRequired();

            builder.Property(swm => swm.UserId)
                .IsRequired();

            builder.Property(swm => swm.AccessLevel)
                .IsRequired();

            builder.Property(swm => swm.JoinedAt)
                .IsRequired();


            builder.HasOne(swm => swm.Wallet)
                .WithMany(sw => sw.Members)
                .HasForeignKey(swm => swm.SharedWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(swm => swm.UserProfile)
                .WithMany()
                .HasForeignKey(swm => swm.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(swm => swm.SharedWalletId)
                .HasDatabaseName("IX_SharedWalletMembers_SharedWalletId");

            builder.HasIndex(swm => swm.UserId)
                .HasDatabaseName("IX_SharedWalletMembers_UserId");

            builder.HasIndex(swm => new { swm.UserId, swm.SharedWalletId })
                .HasDatabaseName("IX_SharedWalletMembers_UserId_SharedWalletId")
                .IsUnique(); // Prevents duplicate memberships
        }
    }
}