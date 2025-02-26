
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.ExpenseId);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(true);

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(8,2)")
                .IsRequired();
            builder.HasCheckConstraint("CK_Expense_Amount_GreaterThanZero", "Amount > 0");

            builder.Property(e => e.Date)
                .IsRequired();

            builder.Property(e => e.CategoryId)
                .IsRequired();

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(true)
                .IsRequired(false);


            builder.Property(e => e.IsAutomated)
                .HasDefaultValue(false);



            builder.HasCheckConstraint(
                "CK_Expense_WalletOrSharedWallet",
                "(WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL)");


            // Relationships
            builder.HasOne(e => e.Wallet)
                .WithMany(wallet => wallet.Expenses)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SharedWallet)
                .WithMany(sharedWallet => sharedWallet.Expenses)
                .HasForeignKey(e => e.SharedWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SubCategory)
                .WithMany()
                .HasForeignKey(e => e.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.RelatedBudget)
                .WithMany()
                .HasForeignKey(e => e.RelatedBudgetId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Template)
                .WithMany()
                .HasForeignKey(e => e.TemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(e => e.Attachments)
                .WithOne(attachment => attachment.Expense)
                .HasForeignKey("ExpenseId")
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(e => e.StaticPaymentMethodsData)
                .WithMany()
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
        }
    }
}