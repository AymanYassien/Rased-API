using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Subscriptions;

namespace Rased.Infrastructure.Data.Config.Subscriptions
{
    public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.ToTable("Subscriptions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.StartDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.EndDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(20)")
                   .IsRequired(false);
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.User)
                    .WithMany(x => x.Subscriptions)
                    .HasForeignKey(x => x.UserId)
                    .IsRequired();
        }
    }
}
