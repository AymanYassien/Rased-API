using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Models.Transfer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Data.Config.TransferConfigures
{
    public class TransactionRejectionConfiguration : IEntityTypeConfiguration<TransactionRejection>
    {
        public void Configure(EntityTypeBuilder<TransactionRejection> builder)
        {
          
            builder.HasKey(tr => tr.RejectionId);

          
            builder.Property(tr => tr.TransactionId)
                .IsRequired();

            builder.Property(tr => tr.RejectedById)
                .IsRequired();

            builder.Property(tr => tr.RejectedAt)
                .IsRequired();

           
            builder.HasOne(tr => tr.Transaction)
                .WithMany()
                .HasForeignKey(tr => tr.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);  

            builder.HasOne(tr => tr.RejectedBy)
                .WithMany()
                .HasForeignKey(tr => tr.RejectedById)
                .OnDelete(DeleteBehavior.Cascade);  

      
            builder.HasIndex(tr => tr.TransactionId)
                .HasDatabaseName("IX_TransactionRejection_TransactionId");

            builder.HasIndex(tr => tr.RejectedById)
                .HasDatabaseName("IX_TransactionRejection_RejectedById");

            builder.HasIndex(tr => new { tr.TransactionId, tr.RejectedById })
                .HasDatabaseName("IX_TransactionRejection_TransactionId_RejectedById")
                .IsUnique(); 
        }
    }

}
