using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Debts;

namespace Rased.Infrastructure.Data.Config.Debts
{
    public class LoanInstallmentConfiguration : IEntityTypeConfiguration<LoanInstallment>
    {
        public void Configure(EntityTypeBuilder<LoanInstallment> builder)
        {
            builder.ToTable("LoanInstallments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.AmountToPay)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired(false);
            builder.Property(x => x.DateToPay)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);
        }
    }
}
