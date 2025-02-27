using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class ExpenseTemplateConfiguration : IEntityTypeConfiguration<ExpenseTemplate>
    {
        public void Configure(EntityTypeBuilder<ExpenseTemplate> builder)
        {
            builder.HasKey(e => e.TemplateId);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(e => e.AutomationRuleId)
                .IsRequired();

            builder.Property(e => e.CategoryId)
                .IsRequired();

            builder.Property(e => e.Amount)
                .HasColumnType("decimal(8,2)")
                .IsRequired()
                .HasPrecision(8, 2);
            builder.HasCheckConstraint("CK_ExpenseTemplate_Amount_Positive", "[Amount] >= 0");

            builder.Property(e => e.SubCategoryId)
                .IsRequired(false);

            builder.Property(e => e.IsNeedApprovalWhenAutoAdd)
                .HasDefaultValue(false);

            builder.Property(e => e.Description)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)")
                .IsRequired(false);

            builder.Property(e => e.PaymentMethodId)
                .IsRequired(false);

            builder.HasCheckConstraint(
                "CK_Expense_WalletOrSharedWallet",
                "(WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL)");

            // Relationships
            builder.HasOne(e => e.Wallet)
                .WithMany(wallet => wallet.ExpensesTemplate)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SharedWallet)
                .WithMany(sharedWallet => sharedWallet.ExpensesTemplates)
                .HasForeignKey(e => e.SharedWalletId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.SubCategory)
                .WithMany()
                .HasForeignKey(e => e.SubCategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);
            
            builder.HasOne(e => e.StaticPaymentMethodsData)
                .WithMany()
                .HasForeignKey(e => e.PaymentMethodId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(e => e.AutomationRule)
                .WithMany()
                .HasForeignKey(e => e.AutomationRuleId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes 
            builder.HasIndex(e => e.Name)
                .HasDatabaseName("IX_ExpenseTemplate_Name");

            builder.HasIndex(e => e.AutomationRuleId)
                .HasDatabaseName("IX_ExpenseTemplate_AutomationRuleId");
        }
    }

}