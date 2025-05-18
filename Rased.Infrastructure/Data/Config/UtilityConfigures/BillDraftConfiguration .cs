using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Models.Bills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Data.Config.UtilityConfigures
{
    public class BillDraftConfiguration : IEntityTypeConfiguration<BillDraft>
    {
        public void Configure(EntityTypeBuilder<BillDraft> builder)
        {
            builder.ToTable("BillDrafts");

            
            builder.HasKey(x => x.BillDraftId);

            builder.Property(x => x.BillDraftId)
                   .ValueGeneratedOnAdd();  // سيتم توليد هذا المفتاح تلقائيًا عند إضافة السجل

         
            builder.Property(x => x.Amount)
                   .HasColumnType("DECIMAL(18, 2)")  
                   .IsRequired(); 

          

            builder.Property(x => x.Date)
                   .HasColumnType("DATETIME2")  
                   .IsRequired();  

            builder.Property(x => x.Description)
                   .HasColumnType("NVARCHAR(MAX)")  
                   .IsRequired(false);

            builder.HasOne(x => x.Wallet)
                 .WithMany(w => w.BillDrafts)
                 .HasForeignKey(x => x.WalletId)
                 .OnDelete(DeleteBehavior.NoAction);  // استخدم NoAction بدلاً من Cascade

            builder.HasOne(x => x.SharedWallet)
                   .WithMany(sw => sw.BillDrafts)
                   .HasForeignKey(x => x.SharedWalletId)
                   .OnDelete(DeleteBehavior.NoAction);  // استخدم NoAction بدلاً من Cascade

            // العلاقة بين BillDraft و Attachments (واحد إلى متعدد)
            builder.HasMany(x => x.Attachments)
                   .WithOne(a => a.BillDraft)
                   .HasForeignKey(a => a.BillDraftId)
                   .OnDelete(DeleteBehavior.NoAction);  // يتم حذف المرفقات تلق
        }
    }
}