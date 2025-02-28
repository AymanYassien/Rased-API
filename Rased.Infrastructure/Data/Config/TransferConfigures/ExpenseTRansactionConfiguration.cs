using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class ExpenseTransactionRecordConfiguration : IEntityTypeConfiguration<ExpenseTransactionRecord>
    {
        public void Configure(EntityTypeBuilder<ExpenseTransactionRecord> builder)
        {
            builder.HasKey(etr => etr.ExpenseTrasactionRecordId);


            builder.Property(etr => etr.TransactionId)
                .IsRequired();

            builder.Property(etr => etr.ExpenseId)
                .IsRequired();

            builder.Property(etr => etr.isDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(etr => etr.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(etr => etr.UpdatedAt)
                .IsRequired(false);

            builder.Property(etr => etr.ExpenseSpecificData)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");


            builder.HasOne(etr => etr.Transaction)
                .WithMany()
                .HasForeignKey(etr => etr.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(etr => etr.Expense)
                .WithMany()
                .HasForeignKey(etr => etr.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(etr => etr.TransactionId)
                .HasDatabaseName("IX_ExpenseTransactionRecord_TransactionId");

            builder.HasIndex(etr => etr.ExpenseId)
                .HasDatabaseName("IX_ExpenseTransactionRecord_ExpenseId");

            builder.HasIndex(etr => new { etr.TransactionId, etr.ExpenseId })
                .HasDatabaseName("IX_ExpenseTransactionRecord_TransactionId_ExpenseId")
                .IsUnique(); // Prevents duplicate links

        }
    }
}