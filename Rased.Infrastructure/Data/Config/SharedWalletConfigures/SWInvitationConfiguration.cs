using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.SharedWallets;

namespace Rased.Business
{
    public class SWInvitationConfiguration : IEntityTypeConfiguration<SWInvitation>
    {
        public void Configure(EntityTypeBuilder<SWInvitation> builder)
        {

            builder.ToTable("SWInvitations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(50)")
                   .HasDefaultValue(InvitationStatusConstants.PENDING);
            builder.Property(x => x.InvitedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            // Relationships
            builder.HasOne(x => x.SharedWallet)
                   .WithMany(x => x.SWInvitations)
                   .HasForeignKey(x => x.SharedWalletId)
                   .IsRequired();
            builder.HasOne(x => x.Sender)
                     .WithMany(x => x.Senders)
                     .HasForeignKey(x => x.SenderId)
                     .OnDelete(DeleteBehavior.NoAction) // No action on delete to prevent cascading delete
                     .IsRequired();
            builder.HasOne(x => x.Receiver)
                     .WithMany(x => x.Receivers)
                     .HasForeignKey(x => x.ReceiverId)
                     .OnDelete(DeleteBehavior.NoAction) // No action on delete to prevent cascading delete
                     .IsRequired();
        }
    }
}