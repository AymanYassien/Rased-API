using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

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
                .HasColumnType("nvarchar(50)");

            builder.Property(sw => sw.Description)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(500)");

            builder.Property(sw => sw.Color)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(sw => sw.Icon)
                .HasMaxLength(100)
                .HasColumnType("nvarchar(100)");

            builder.Property(sw => sw.Currency)
                .IsRequired()
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            builder.Property(sw => sw.TotalBalance)
                .HasColumnType("decimal(8,2)")
                .IsRequired();

            builder.Property(sw => sw.CreatorId)
                .IsRequired();

            builder.Property(sw => sw.CreatedAt)
                .IsRequired();

            builder.Property(sw => sw.LastModified)
                .IsRequired();

            builder.Property(sw => sw.WalletStatusId)
                .IsRequired();


            builder.HasOne(sw => sw.StaticWalletStatusData)
                .WithMany()
                .HasForeignKey(sw => sw.WalletStatusId)
                .OnDelete(DeleteBehavior.Restrict);
            
            builder.HasOne(sw => sw.Creator)
                .WithMany()
                .HasForeignKey(sw => sw.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(sw => sw.Members)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Incomes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Expenses)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.ExpensesTemplates)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Budgets)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Loans)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Goals)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.Savings)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.SentTransactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sw => sw.ReceivedTransactions)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}