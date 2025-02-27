using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class AutomationRuleConfiguration : IEntityTypeConfiguration<AutomationRule>
{
    public void Configure(EntityTypeBuilder<AutomationRule> entity)
    {
        entity.HasKey(e => e.AutomationRuleId);

        
        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        entity.Property(e => e.Description)
            .HasMaxLength(200)
            .IsRequired(false)
            .HasColumnType("nvarchar(200)");

        entity.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        entity.Property(e => e.StartDate)
            .IsRequired();

        entity.Property(e => e.EndDate)
            .IsRequired();

        entity.Property(e => e.DayOfMonth)
            .IsRequired(false);

        entity.Property(e => e.DayOfWeek)
            .IsRequired(false);
        //entity.HasOne(e => e.Wallet)
        //    .WithMany()
        //    .HasForeignKey(e => e.WalletId)
        //    .IsRequired(false); 

        //entity.HasOne(e => e.SharedWallet)
        //    .WithMany() 
        //    .HasForeignKey(e => e.SharedWalletId)
        //    .IsRequired(false); 
        
        entity.HasOne(e => e.StaticTriggerTypeData)
            .WithMany() 
            .HasForeignKey(e => e.TriggerTypeId)
            .IsRequired();
    }
}