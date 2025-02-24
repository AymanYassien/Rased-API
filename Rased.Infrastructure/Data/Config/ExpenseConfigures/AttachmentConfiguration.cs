
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Expenses;

namespace Rased.Infrastructure.Data.Config.ExpenseConfigures
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {

            builder.HasKey(a => a.AttachmentId);

            builder.Property(a => a.ExpenseId)
                .IsRequired();

            builder.Property(a => a.FilePath)
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("nvarchar(512)");

            builder.Property(a => a.FileName)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnType("nvarchar(50)");

            builder.Property(a => a.FileType)
                .IsRequired(false)
                .HasMaxLength(10)
                .HasColumnType("nvarchar(10)");

            builder.Property(a => a.FileSize)
                .IsRequired(false)
                .HasColumnType("bigint");

            builder.Property(a => a.UploadDate)
                .IsRequired()
                .HasColumnType("datetime");

            // Relationship
            builder.HasOne(a => a.Expense)
                .WithMany()
                .HasForeignKey(a => a.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

            // Index 
            builder.HasIndex(a => a.ExpenseId)
                .HasDatabaseName("IX_Attachment_ExpenseId");
        }
    }
}
