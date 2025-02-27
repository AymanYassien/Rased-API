
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
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

            builder.Property(b => b.BudgetTypeId)
                .IsRequired();


            builder.Property(b => b.SpentAmount)
                .HasColumnType("decimal(8,2)")
                .HasDefaultValue(0.00m);

            builder.Property(b => b.RemainingAmount)
                .HasPrecision(8, 2)
                .HasDefaultValueSql("PlannedAmount");


            builder.Property(b => b.DayOfMonth)
                .HasAnnotation("Range", new[] { 1, 28 });

            builder.Property(b => b.DayOfWeek)
                .HasAnnotation("Range", new[] { 1, 7 });


            // DEfault values for none Required 
            builder.Property(b => b.RolloverUnspent)
                .HasDefaultValue(false);


            builder.HasOne(b => b.Wallet)
                .WithMany()
                .HasForeignKey(b => b.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SharedWallet)
                .WithMany()
                .HasForeignKey(b => b.SharedWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Category)
                .WithMany()
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(b => b.StaticBudgetTypesData)
                .WithMany()
                .HasForeignKey(b => b.BudgetTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SubCategory)
                .WithMany()
                .HasForeignKey(b => b.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);


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