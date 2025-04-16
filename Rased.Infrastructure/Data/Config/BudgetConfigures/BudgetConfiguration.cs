
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
            builder.Property(b => b.CategoryName)
                .IsRequired(false)
                .HasColumnType("nvarchar(50)");

            builder.Property(b => b.BudgetAmount)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.Property(b => b.BudgetTypeId)
                .IsRequired(false);


            builder.Property(b => b.SpentAmount)
                .HasColumnType("decimal(8,2)")
                .HasDefaultValue(0.00m);

            builder.Property(b => b.RemainingAmount)
                .HasPrecision(8, 2);


            builder.Property(b => b.DayOfMonth);

            builder.Property(b => b.DayOfWeek);

            // DEfault values for none Required 
            builder.Property(b => b.RolloverUnspent)
                .HasDefaultValue(false);


            builder.HasOne(b => b.Wallet)
                .WithMany(x => x.Budgets)
                .HasForeignKey(b => b.WalletId)
                .IsRequired(false);

            builder.HasOne(b => b.SharedWallet)
                .WithMany(x => x.Budgets)
                .HasForeignKey(b => b.SharedWalletId)
                .IsRequired(false);
            
            builder.HasOne(b => b.StaticBudgetTypesData)
                .WithMany()
                .HasForeignKey(b => b.BudgetTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.SubCategory)
                .WithMany(x => x.Budgets)
                .HasForeignKey(b => b.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);


            //builder.HasCheckConstraint("CK_Budget_BudgetAmount_Positive",
            //    "[BudgetAmount] > 0");

            builder.HasCheckConstraint("CK_Budget_SpentAmount_NonNegative",
                "[SpentAmount] >= 0");

            builder.HasCheckConstraint("CK_Budget_DateRange_Valid",
                "[EndDate] > [StartDate]");

            builder.HasCheckConstraint("CK_Budget_WalletAssignment",
                "([WalletId] IS NULL AND [SharedWalletId] IS NOT NULL) OR ([WalletId] IS NOT NULL AND [SharedWalletId] IS NULL)");
        }
    }
}