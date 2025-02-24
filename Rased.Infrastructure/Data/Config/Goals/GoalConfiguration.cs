using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Goals;

namespace Rased.Infrastructure.Data.Config.Goals
{
    public class GoalConfiguration : IEntityTypeConfiguration<Goal>
    {
        public void Configure(EntityTypeBuilder<Goal> builder)
        {
            builder.ToTable("Goals");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Description)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false);
            builder.Property(x => x.Status)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired(false);
            builder.Property(x => x.StartedDate)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.CurrentAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.TargetAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.StartedDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.DesiredDate)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.IsTemplate)
                   .HasColumnType("BIT")
                   .HasDefaultValue(0);
            builder.Property(x => x.Frequency)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired(false);
            builder.Property(x => x.FrequencyAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired(false);
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.Wallet)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired();
            builder.HasOne(x => x.SharedWallet)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.SharedWalletId)
                   .IsRequired();
            builder.HasOne(x => x.SubCategory)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.WalletId)
                   .IsRequired();
            builder.HasMany(x => x.GoalTransactions)
                   .WithOne(x => x.Goal)
                    .HasForeignKey(x => x.GoalId)
                    .IsRequired();
        }
    }
}
