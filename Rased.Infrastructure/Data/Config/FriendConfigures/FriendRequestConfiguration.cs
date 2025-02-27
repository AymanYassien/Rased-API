using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business
{
    public class FriendRequestConfiguration : IEntityTypeConfiguration<FriendRequest>
    {
        public void Configure(EntityTypeBuilder<FriendRequest> builder)
        {
            builder.HasKey(fr => fr.RequestId);

            builder.Property(fr => fr.SenderId)
                .IsRequired();

            builder.Property(fr => fr.ReceiverId)
                .IsRequired();

            builder.Property(fr => fr.SentAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(fr => fr.FriendRequestStatusId)
                .IsRequired();

            builder.Property(fr => fr.UpdatedAt)
                .IsRequired(false)
                .HasColumnType("datetime");

            builder.HasOne(fr => fr.Sender)
                .WithMany()
                .HasForeignKey(fr => fr.SenderId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(fr => fr.StaticFriendRequestStatusData)
                .WithMany()
                .HasForeignKey(fr => fr.FriendRequestStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(fr => fr.Receiver)
                .WithMany()
                .HasForeignKey(fr => fr.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(fr => fr.SenderId)
                .HasDatabaseName("IX_FriendRequest_SenderId");

            builder.HasIndex(fr => fr.ReceiverId)
                .HasDatabaseName("IX_FriendRequest_ReceiverId");
        }
    }
}