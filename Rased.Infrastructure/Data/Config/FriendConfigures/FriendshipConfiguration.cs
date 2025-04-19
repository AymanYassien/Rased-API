using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.ToTable("Friendships");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(100)")
                   .IsRequired();
            builder.Property(x => x.SentAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            // Relationships
            builder.HasOne(x => x.Sender)
                     .WithMany(x => x.FriendSenders)
                     .HasForeignKey(x => x.SenderId)
                     .IsRequired();
            builder.HasOne(x => x.Receiver)
                     .WithMany(x => x.FriendReceivers)
                     .HasForeignKey(x => x.ReceiverId)
                     .OnDelete(DeleteBehavior.NoAction) // No action on delete to prevent cascading delete
                     .IsRequired();
        }
    }

}
