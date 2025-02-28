using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasKey(f => f.FriendshipId);

            builder.Property(f => f.UserId1)
                .IsRequired();

            builder.Property(f => f.UserId2)
                .IsRequired();

            builder.Property(f => f.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime");

            builder.Property(f => f.Nickname1)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(f => f.Nickname2)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(f => f.FriendshipStatusId)
                .IsRequired();

            builder.HasOne(f => f.User1Profile)
                .WithMany()
                .HasForeignKey(f => f.UserId1)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(f => f.StaticFriendshipStatusData)
                .WithMany()
                .HasForeignKey(f => f.FriendshipStatusId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.User2Profile)
                .WithMany()
                .HasForeignKey(f => f.UserId2)
                .OnDelete(DeleteBehavior.NoAction);


            builder.HasIndex(f => f.UserId1)
                .HasDatabaseName("IX_Friendship_UserId1");

            builder.HasIndex(f => f.UserId2)
                .HasDatabaseName("IX_Friendship_UserId2");

            builder.HasIndex(f => new { f.UserId1, f.UserId2 })
                .IsUnique()
                .HasDatabaseName("IX_Friendship_UserId1_UserId2");
        }
    }

}
