using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Bills;
using Rased.Infrastructure.Models.Recomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Data.Config.UtilityConfigures
{
    public class BudgetRecommendationConfiguration : IEntityTypeConfiguration<BudgetRecommendation>
    {
        public void Configure(EntityTypeBuilder<BudgetRecommendation> entity)
        {

            
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.GeneratedAt)
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.IsRead)
                    .HasDefaultValue(false);

                entity.HasOne(e => e.User)
                    .WithMany()
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Wallet)
                    .WithMany()
                    .HasForeignKey(e => e.WalletId)
                    .OnDelete(DeleteBehavior.Cascade); 

                entity.HasOne(e => e.WalletGroup)
                    .WithMany()
                    .HasForeignKey(e => e.WalletGroupId)
                    .OnDelete(DeleteBehavior.Cascade); 


        }


    }
}
