using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Debts;

namespace Rased.Infrastructure.Data.Config.Debts
{
    public class LoanConfiguration : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.ToTable("Loans");
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
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.PaidAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.InstallmentAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.TotalInstallments)
                   .HasColumnType("INT")
                   .IsRequired();
            builder.Property(x => x.PaidInstallments)
                   .HasColumnType("INT")
                   .IsRequired();
            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired(false);
            builder.Property(x => x.StartDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.EndDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.Wallet)
                   .WithMany(x => x.Loans)
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired();
            builder.HasMany(x => x.LoanInstallments)
                   .WithOne(x => x.Loan)
                   .HasForeignKey(x => x.LoanId)
                   .IsRequired();
        }
    }
}
