
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Budgets;


namespace Rased.Infrastructure.Data.Config.BudgetConfigures
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {

            builder.HasKey(b => b.BudgetId);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("nvarchar(50)");

            builder.Property(b => b.CategoryId)
                .IsRequired();

            builder.Property(b => b.PlannedAmount)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.Status)
                .IsRequired();


            builder.Property(b => b.SpentAmount)
                .HasColumnType("decimal(8,2)")
                .HasDefaultValue(0.00m);

            builder.Property(b => b.RemainingAmount)
                .HasPrecision(7, 2)
                .HasDefaultValueSql("PlannedAmount");


            builder.Property(b => b.DayOfMonth)
                .HasAnnotation("Range", new[] { 1, 28 });

            builder.Property(b => b.DayOfWeek)
                .HasAnnotation("Range", new[] { 1, 7 });


            // DEfault values for none Required 
            builder.Property(b => b.RolloverUnspent)
                .HasDefaultValue(false);


            builder.HasOne(b => b.Wallet)
                .WithMany(x => x.Budgets)
                .HasForeignKey(b => b.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SharedWallet)
                .WithMany()
                .HasForeignKey(b => b.SharedWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SubCategory)
                .WithMany(x => x.Budgets)
                .HasForeignKey(b => b.SubCategoryId)
                .IsRequired();


            builder.HasCheckConstraint("CK_Budget_PlannedAmount_Positive",
                "[PlannedAmount] > 0");

            builder.HasCheckConstraint("CK_Budget_SpentAmount_NonNegative",
                "[SpentAmount] >= 0");

            builder.HasCheckConstraint("CK_Budget_DateRange_Valid",
                "[EndDate] > [StartDate]");

            builder.HasCheckConstraint("CK_Budget_WalletAssignment",
                "([WalletId] IS NULL AND [SharedWalletId] IS NOT NULL) OR ([WalletId] IS NOT NULL AND [SharedWalletId] IS NULL)");
        }
    }
}