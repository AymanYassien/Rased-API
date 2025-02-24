using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Savings;

namespace Rased.Infrastructure.Data.Config.Savings
{
    public class SavingConfiguration : IEntityTypeConfiguration<Saving>
    {
        public void Configure(EntityTypeBuilder<Saving> builder)
        {
            builder.ToTable("Savings");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Description)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false);
            builder.Property(x => x.TotalAmount)
                   .HasColumnType("DECIMAL(9, 6)")
                   .IsRequired(false);
            builder.Property(x => x.IsSaving)
                   .HasColumnType("BIT")
                   .HasDefaultValue(1);
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.Wallet)
                   .WithMany(x => x.Savings)
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired();
            builder.HasOne(x => x.SharedWallet)
                   .WithMany(x => x.Savings)
                   .HasForeignKey(x => x.SharedWalletId)
                   .IsRequired();
            builder.HasOne(x => x.SubCategory)
                   .WithMany(x => x.Savings)
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired();
        }
    }
}
