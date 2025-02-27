using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class WalletStatisticsConfiguration : IEntityTypeConfiguration<WalletStatistics>
{
    public void Configure(EntityTypeBuilder<WalletStatistics> entity)
    {
        entity.HasKey(e => e.WalletId);
        
        
        entity.Property(e => e.TotalIncome)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false); 

        entity.Property(e => e.TotalExpenses)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false); 

        entity.Property(e => e.TotalSavings)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false); 

        entity.Property(e => e.TotalLoans)
            .HasColumnType("decimal(18,2)")
            .IsRequired(false); 
        
        entity.HasOne(e => e.Wallet)
            .WithOne(x => x.WalletStatistics)
            .HasForeignKey<WalletStatistics>(e => e.WalletId)
            .IsRequired(false);
        
    }
}