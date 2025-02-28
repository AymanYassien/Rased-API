using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business.IncomesConfigures;

public class IncomeConfiguration : IEntityTypeConfiguration<Income>
{
    public void Configure(EntityTypeBuilder<Income> entity)
    {
        entity.HasKey(e => e.IncomeId);
        
        
        entity.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(50);
        entity.Property(e => e.CategoryName)
            .IsRequired(false)
            .HasMaxLength(50);

        entity.Property(e => e.Description)
            .HasMaxLength(200)
            .IsRequired(false);

        entity.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(8,2)");

        entity.Property(e => e.CreatedDate)
            .IsRequired()
            .HasDefaultValueSql("GETUTCDATE()"); 

        entity.Property(e => e.IsAutomated)
            .IsRequired()
            .HasDefaultValue(false);
        
        entity.HasOne(e => e.Wallet)
            .WithMany(w => w.Incomes)
            .HasForeignKey(e => e.WalletId)
            .IsRequired(false); 

        entity.HasOne(e => e.SharedWallet)
            .WithMany(x => x.Incomes) 
            .HasForeignKey(e => e.SharedWalletId)
            .IsRequired(false);

        entity.HasOne(e => e.SubCategory)
            .WithMany(x => x.Incomes)
            .HasForeignKey(e => e.SubCategoryId)
            .IsRequired(false);

        entity.HasOne(e => e.IncomeSourceType)
            .WithMany() 
            .HasForeignKey(e => e.IncomeSourceTypeId)
            .IsRequired();

        //entity.HasOne(e => e.IncomeTemplate)
        //    .WithMany() 
        //    .HasForeignKey(e => e.IncomeTemplateId)
        //    .IsRequired(false); 

        
        entity.HasCheckConstraint("CK_Income_WalletOrSharedWallet",
            "((WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL))");
    }
}