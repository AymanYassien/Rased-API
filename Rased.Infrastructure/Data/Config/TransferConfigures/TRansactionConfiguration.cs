using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(t => t.TransactionId);

            builder.Property(t => t.SenderId)
                .IsRequired();

            builder.Property(t => t.SenderWalletId)
                .IsRequired();

            builder.Property(t => t.ReceiverId)
                .IsRequired(false);

            builder.Property(t => t.ReceiverWalletId)
                .IsRequired(false);

            builder.Property(t => t.ReceiverTypeId)
                .IsRequired();

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("decimal(8,2)");

            builder.Property(t => t.Description)
                .HasMaxLength(200)
                .HasColumnType("nvarchar(200)");

            builder.Property(t => t.CreatedAt)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(t => t.UpdatedAt)
                .IsRequired(false);

            builder.Property(t => t.TransactionStatusId)
                .IsRequired();

            builder.Property(t => t.DisplayColor)
                .HasMaxLength(20)
                .HasColumnType("nvarchar(20)");

            builder.Property(t => t.IsReadOnly)
                .IsRequired()
                .HasDefaultValue(false);


            builder.HasCheckConstraint("CK_Transaction_Amount", "[Amount] > 0");


            builder.HasOne(t => t.Sender)
                .WithMany()
                .HasForeignKey(t => t.SenderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.StaticReceiverTypeData)
                .WithMany()
                .HasForeignKey(t => t.ReceiverTypeId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(t => t.StaticTransactionStatusData)
                .WithMany()
                .HasForeignKey(t => t.TransactionStatusId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(t => t.SenderWallet)
                .WithMany(sw => sw.SentTransactions)
                .HasForeignKey(t => t.SenderWalletId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(t => t.Receiver)
                .WithMany()
                .HasForeignKey(t => t.ReceiverId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);

            builder.HasOne(t => t.ReceiverWallet)
                .WithMany(sw => sw.ReceivedTransactions)
                .HasForeignKey(t => t.ReceiverWalletId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired(false);


            builder.HasIndex(t => t.SenderId)
                .HasDatabaseName("IX_Transaction_SenderId");

            builder.HasIndex(t => t.SenderWalletId)
                .HasDatabaseName("IX_Transaction_SenderWalletId");

            builder.HasIndex(t => t.ReceiverId)
                .HasDatabaseName("IX_Transaction_ReceiverId");

            builder.HasIndex(t => t.ReceiverWalletId)
                .HasDatabaseName("IX_Transaction_ReceiverWalletId");

            builder.HasIndex(t => t.TransactionStatusId)
                .HasDatabaseName("IX_Transaction_Status");
        }
    }

}