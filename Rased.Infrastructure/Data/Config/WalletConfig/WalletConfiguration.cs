using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> entity)
    {
        entity.HasKey(e => e.WalletId);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        entity.Property(e => e.Description)
            .HasMaxLength(200);

        entity.Property(e => e.Icon)
            .HasMaxLength(100);

        entity.Property(e => e.InitialBalance)
            .IsRequired()
            .HasColumnType("decimal(11,2)")
            .HasPrecision(8,2);

        entity.Property(e => e.TotalBalance)
            .IsRequired()
            .HasColumnType("decimal(11,2)")
            .HasPrecision(8, 2);

        entity.Property(e => e.ExpenseLimit)
            .HasColumnType("decimal(11,2)");

        entity.HasOne(e => e.StaticWalletStatusData)
            .WithMany(x => x.Wallets)
            .HasForeignKey(e => e.WalletStatusId);

        entity.HasOne(e => e.StaticColorTypeData)
            .WithMany(x => x.Wallets)
            .HasForeignKey(e => e.ColorTypeId);
    }
}