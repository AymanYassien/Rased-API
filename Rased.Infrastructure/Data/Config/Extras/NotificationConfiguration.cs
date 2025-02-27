using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Extras;

namespace Rased.Infrastructure.Data.Config.Extras
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.ToTable("Notifications");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.TitleEn)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.TitleAr)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.MessageEn)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.MessageAr)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.url)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.User)
                   .WithMany(x => x.Notifications)
                   .HasForeignKey(x => x.UserId)
                   .IsRequired();
        }
    }
}
