using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Transfers;

namespace Rased.Infrastructure.Data.Config.TransferConfigures
{
    public class PersonalIncomeTransactionRecordConfiguration : IEntityTypeConfiguration<PersonalIncomeTrasactionRecord>
    {
        public void Configure(EntityTypeBuilder<PersonalIncomeTrasactionRecord> builder)
        {

            builder.HasKey(pitr => pitr.PersonalIncomeTrasactionRecordId);

            builder.Property(pitr => pitr.TransactionId)
                .IsRequired();

            builder.Property(pitr => pitr.IncomeId)
                .IsRequired();

            builder.Property(pitr => pitr.ApprovalId)
                .IsRequired();

            builder.Property(pitr => pitr.isDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(pitr => pitr.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(pitr => pitr.UpdatedAt)
                .IsRequired(false);

            builder.Property(pitr => pitr.IncomeSpecificData)
                .HasMaxLength(500)
                .HasColumnType("nvarchar(500)");


            builder.HasOne(pitr => pitr.Transaction)
                .WithMany()
                .HasForeignKey(pitr => pitr.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pitr => pitr.Income)
                .WithMany()
                .HasForeignKey(pitr => pitr.IncomeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(pitr => pitr.TransactionApproval)
                .WithMany()
                .HasForeignKey(pitr => pitr.ApprovalId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(pitr => pitr.TransactionId)
                .HasDatabaseName("IX_PersonalIncomeTransactionRecord_TransactionId");

            builder.HasIndex(pitr => pitr.IncomeId)
                .HasDatabaseName("IX_PersonalIncomeTransactionRecord_IncomeId");

            builder.HasIndex(pitr => pitr.ApprovalId)
                .HasDatabaseName("IX_PersonalIncomeTransactionRecord_ApprovalId");

            builder.HasIndex(pitr => new { pitr.TransactionId, pitr.IncomeId, pitr.ApprovalId })
                .HasDatabaseName("IX_PersonalIncomeTransactionRecord_TransactionId_IncomeId_ApprovalId")
                .IsUnique(); // Prevents duplicate links
        }
    }
}