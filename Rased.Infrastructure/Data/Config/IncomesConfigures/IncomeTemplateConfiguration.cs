using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business.IncomesConfigures;



public class IncomeTemplateConfiguration : IEntityTypeConfiguration<IncomeTemplate>
{
    public void Configure(EntityTypeBuilder<IncomeTemplate> entity)
    {
        entity.HasKey(e => e.IncomeTemplateId);

        
        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        entity.Property(e => e.CategoryName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(e => e.Description)
            .HasMaxLength(200)
            .IsRequired(false)
            .HasColumnType("nvarchar(200)");

        entity.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(8,2)");

        entity.Property(e => e.IsNeedApprovalWhenAutoAdd)
            .IsRequired()
            .HasDefaultValue(false);
        
        entity.HasOne(e => e.Wallet)
            .WithMany(x => x.IncomeTemplates)
            .HasForeignKey(e => e.WalletId)
            .IsRequired(false); 

        entity.HasOne(e => e.SharedWallet)
            .WithMany(x => x.IncomeTemplates)
            .HasForeignKey(e => e.SharedWalletId)
            .IsRequired(false);

        entity.HasOne(e => e.SubCategory)
            .WithMany(x => x.IncomeTemplates)
            .HasForeignKey(e => e.SubCategoryId)
            .IsRequired(false);

        entity.HasOne(e => e.AutomationRule)
            .WithOne(x => x.IncomeTemplate) 
            .HasForeignKey<IncomeTemplate>(e => e.AutomationRuleId)
            .IsRequired();

        entity.HasOne(e => e.IncomeSourceType)
            .WithMany()
            .HasForeignKey(e => e.IncomeSourceTypeId)
            .IsRequired();
        
        entity.HasCheckConstraint("CK_IncomeTemplate_WalletOrSharedWallet",
            "((WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL))");
    }
}