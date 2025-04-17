using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.SharedWallets;

namespace Rased.Business
{
    public class SharedWalletConfiguration : IEntityTypeConfiguration<SharedWallet>
    {
        public void Configure(EntityTypeBuilder<SharedWallet> builder)
        {

            builder.HasKey(sw => sw.SharedWalletId);

            builder.Property(sw => sw.Name)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)")
                .IsRequired();

            builder.Property(sw => sw.Description)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(500)")
                .IsRequired(false);

            builder.Property(sw => sw.Icon)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)")
                .IsRequired(false);

            builder.Property(sw => sw.TotalBalance)
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            builder.Property(sw => sw.InitialBalance)
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            builder.Property(sw => sw.ExpenseLimit)
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            //builder.Property(sw => sw.CreatorId)
            //    .IsRequired();

            builder.Property(sw => sw.CreatedAt)
                .IsRequired();

            builder.Property(sw => sw.LastModified)
                .IsRequired(false);

            builder.Property(sw => sw.WalletStatusId)
                .IsRequired();

            builder.HasOne(e => e.StaticColorTypeData)
            .WithMany(x => x.SharedWallets)
            .HasForeignKey(e => e.ColorTypeId);

            builder.HasOne(sw => sw.StaticWalletStatusData)
                .WithMany()
                .HasForeignKey(sw => sw.WalletStatusId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sw => sw.SentTransactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.ReceivedTransactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}