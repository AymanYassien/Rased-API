using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.SharedWallets;


namespace Rased.Business
{
    public class SharedWalletMembersConfiguration : IEntityTypeConfiguration<SharedWalletMembers>
    {
        public void Configure(EntityTypeBuilder<SharedWalletMembers> builder)
        {
            builder.HasKey(swm => swm.MembershipId);

            builder.Property(x => x.Role)
                   .HasColumnType("NVARCHAR(100)")
                   .IsRequired();

            //builder.Property(swm => swm.SharedWalletId)
            //    .IsRequired();

            //builder.Property(swm => swm.UserId)
            //    .IsRequired();

            //builder.Property(swm => swm.AccessLevelId)
            //    .IsRequired();

            builder.Property(swm => swm.JoinedAt)
                .IsRequired();


            //builder.HasOne(swm => swm.SharedWallet)
            //    .WithMany(sw => sw.Members)
            //    .HasForeignKey(swm => swm.SharedWalletId)
            //    .OnDelete(DeleteBehavior.NoAction)
            //    .IsRequired();

            //builder.HasOne(swm => swm.Member)
            //    .WithMany(x => x.SWMembers)
            //    .HasForeignKey(swm => swm.UserId)
            //    .IsRequired();

            //builder.HasOne(swm => swm.StaticSharedWalletAccessLevelData)
            //    .WithMany(x => x.Members)
            //    .HasForeignKey(swm => swm.SharedWalletId)
            //    .IsRequired();
        }
    }
}