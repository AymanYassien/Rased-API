
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.HasKey(a => a.AttachmentId);

            builder.Property(a => a.ExpenseId)
                .IsRequired(false); 

            builder.Property(a => a.BillDraftId)
                .IsRequired(false); 

            builder.Property(a => a.FilePath)
                .IsRequired()
                //.HasMaxLength(512)
                .HasColumnType("nvarchar(MAX)");

            builder.Property(a => a.FileName)
                .IsRequired(false)
                //.HasMaxLength(50)
                .HasColumnType("nvarchar(MAX)");

            builder.Property(a => a.FileType)
                .IsRequired(false)
                //.HasMaxLength(10)
                .HasColumnType("nvarchar(MAX)");

            builder.Property(a => a.FileSize)
                .IsRequired(false)
                .HasColumnType("bigint");

            builder.Property(a => a.UploadDate)
                .IsRequired()
                .HasColumnType("datetime");

     
            builder.HasOne(a => a.Expense)
                .WithMany()
                .HasForeignKey(a => a.ExpenseId)
                .OnDelete(DeleteBehavior.Cascade);

         
            builder.HasOne(a => a.BillDraft)
                .WithMany()
                .HasForeignKey(a => a.BillDraftId)
                .OnDelete(DeleteBehavior.Cascade);

         
            builder.HasIndex(a => a.ExpenseId)
                .HasDatabaseName("IX_Attachment_ExpenseId");

            builder.HasIndex(a => a.BillDraftId)
                .HasDatabaseName("IX_Attachment_BillDraftId");
        }
    }

}
