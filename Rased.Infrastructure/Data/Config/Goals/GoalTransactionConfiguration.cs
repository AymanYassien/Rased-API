using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Goals;

namespace Rased.Infrastructure.Data.Config.Goals
{
    public class GoalTransactionConfiguration : IEntityTypeConfiguration<GoalTransaction>
    {
        public void Configure(EntityTypeBuilder<GoalTransaction> builder)
        {
            builder.ToTable("GoalTransactions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.InsertedAmount)
                   .HasColumnType("DECIMAL(12, 9)")
                   .IsRequired();
            builder.Property(x => x.InsertedDate)
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
