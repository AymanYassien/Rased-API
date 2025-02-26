
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class BudgetAlertConfiguration : IEntityTypeConfiguration<BudgetAlert>
    {
        public void Configure(EntityTypeBuilder<BudgetAlert> builder)
        {
            builder.HasKey(b => b.AlertId);

            builder.Property(b => b.BudgetId)
                .IsRequired();

            builder.Property(b => b.Threshold)
                .HasColumnType("decimal(8,2)")
                .HasPrecision(8, 2)
                .IsRequired();

            builder.HasCheckConstraint("CK_BudgetAlert_Threshold_GreaterThanZero", "Threshold > 0");

            builder.HasCheckConstraint(
                "CK_BudgetAlert_Percentage_Range",
                "([Type] != 'PERCENTAGE') OR ([Type] = 'PERCENTAGE' AND [Threshold] BETWEEN 1 AND 100)"
            );

            builder.Property(b => b.IsEnabled)
                .HasDefaultValue(true);

            builder.HasCheckConstraint("CK_BudgetAlert_Threshold_GreaterThanZero", "Threshold > 0");

            builder.Property(b => b.Type)
                .IsRequired();

            builder.Property(b => b.NotificationType)
                .IsRequired();

            builder.HasOne(b => b.Budget)
                .WithMany(budget => budget.Alerts)
                .HasForeignKey(b => b.BudgetId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

