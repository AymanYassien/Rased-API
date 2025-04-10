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

            builder.Property(x => x.CategoryName)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired(false);

            builder.Property(x => x.Description)
                    .HasConversion<string>()
                   .IsRequired(false);
            builder.Property(x => x.Status)
              .HasColumnType("NVARCHAR(50)")
              .HasDefaultValue(GoalStatusEnum.InProgress) 
              .IsRequired();

            builder.Property(x => x.StartedAmount)
                   .IsRequired()
                   .HasPrecision(18, 2);
            builder.Property(x => x.CurrentAmount)
                   .IsRequired()
                   .HasPrecision(18, 2);
            builder.Property(x => x.TargetAmount)
                   .IsRequired()
                   .HasPrecision(18, 2);
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
                   .HasPrecision(18, 2)
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
                   .IsRequired(false);
            builder.HasOne(x => x.SharedWallet)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.SharedWalletId)
                   .IsRequired(false);
            builder.HasOne(x => x.SubCategory)
                   .WithMany(x => x.Goals)
                   .HasForeignKey(x => x.SubCatId)
                   .IsRequired(false);
            builder.HasMany(x => x.GoalTransactions)
                   .WithOne(x => x.Goal)
                   .HasForeignKey(x => x.GoalId)
                   .IsRequired();
        }
    }
}
