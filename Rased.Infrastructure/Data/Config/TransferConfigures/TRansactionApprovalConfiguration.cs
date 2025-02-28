using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class TransactionApprovalConfiguration : IEntityTypeConfiguration<TransactionApproval>
    {
        public void Configure(EntityTypeBuilder<TransactionApproval> builder)
        {

            builder.HasKey(ta => ta.ApprovalId);


            builder.Property(ta => ta.TransactionId)
                .IsRequired();

            builder.Property(ta => ta.ApproverId)
                .IsRequired();

            builder.Property(ta => ta.ApprovedAt)
                .IsRequired();

            // Relationships
            builder.HasOne(ta => ta.Transaction)
                .WithMany()
                .HasForeignKey(ta => ta.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(ta => ta.Approver)
                .WithMany()
                .HasForeignKey(ta => ta.ApproverId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(ta => ta.TransactionId)
                .HasDatabaseName("IX_TransactionApproval_TransactionId");

            builder.HasIndex(ta => ta.ApproverId)
                .HasDatabaseName("IX_TransactionApproval_ApproverId");

            builder.HasIndex(ta => new { ta.TransactionId, ta.ApproverId })
                .HasDatabaseName("IX_TransactionApproval_TransactionId_ApproverId")
                .IsUnique(); // Prevents duplicate approvals
        }
    }
}