using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class WalletNotificationConfiguration : IEntityTypeConfiguration<WalletNotification>
{
    public void Configure(EntityTypeBuilder<WalletNotification> entity)
    {
        entity.HasKey(e => e.WalletNotificationId);
        
        entity.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()"); 

        entity.Property(e => e.IsRead)
            .IsRequired()
            .HasDefaultValue(false);

        entity.Property(e => e.Threshold)
            .IsRequired()
            .HasColumnType("decimal(8,2)")
            .HasAnnotation("MinValue", 0); 

        entity.Property(e => e.CurrentAmount)
            .HasColumnType("decimal(8,2)"); 
        
        entity.HasOne(e => e.Wallet)
            .WithMany(w => w.Notifications) 
            .HasForeignKey(e => e.WalletId)
            .IsRequired();

        entity.HasOne(e => e.NotificationType)
            .WithMany() 
            .HasForeignKey(e => e.NotificationTypeId)
            .IsRequired();

        entity.HasOne(e => e.ThresholdType)
            .WithMany() 
            .HasForeignKey(e => e.ThresholdTypeId)
            .IsRequired();
    }
}