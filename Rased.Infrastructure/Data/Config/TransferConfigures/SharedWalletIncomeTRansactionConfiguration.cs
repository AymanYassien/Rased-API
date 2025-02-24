using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Transfers;


namespace Rased.Infrastructure.Data.Config.TransferConfigures
{
    public class SharedWalletIncomeTransactionConfiguration : IEntityTypeConfiguration<SharedWalletIncomeTransaction>
    {
        public void Configure(EntityTypeBuilder<SharedWalletIncomeTransaction> builder)
        {
            builder.HasKey(swit => swit.SharedWalletIncomeTransactionId);


            builder.Property(swit => swit.TransactionId)
                .IsRequired();

            builder.Property(swit => swit.IncomeId)
                .IsRequired();

            builder.Property(swit => swit.ApprovalId)
                .IsRequired();

            builder.Property(swit => swit.isDeleted)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(swit => swit.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(swit => swit.UpdatedAt)
                .IsRequired(false);


            builder.HasOne(swit => swit.Transaction)
                .WithMany()
                .HasForeignKey(swit => swit.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(swit => swit.Income)
                .WithMany()
                .HasForeignKey(swit => swit.IncomeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(swit => swit.TransactionApproval)
                .WithMany() // No obvious collection in TransactionApproval
                .HasForeignKey(swit => swit.ApprovalId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(swit => swit.TransactionId)
                .HasDatabaseName("IX_SharedWalletIncomeTransaction_TransactionId");

            builder.HasIndex(swit => swit.IncomeId)
                .HasDatabaseName("IX_SharedWalletIncomeTransaction_IncomeId");

            builder.HasIndex(swit => swit.ApprovalId)
                .HasDatabaseName("IX_SharedWalletIncomeTransaction_ApprovalId");

            builder.HasIndex(swit => new { swit.TransactionId, swit.IncomeId, swit.ApprovalId })
                .HasDatabaseName("IX_SharedWalletIncomeTransaction_TransactionId_IncomeId_ApprovalId")
                .IsUnique(); // Prevents duplicate links
        }
    }
}