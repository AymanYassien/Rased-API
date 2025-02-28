using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Data.Config.Subscriptions
{
    public class PlanConfiguration : IEntityTypeConfiguration<Plan>
    {
        public void Configure(EntityTypeBuilder<Plan> builder)
        {
            builder.ToTable("Plans");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Name)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Description)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false);
            builder.Property(x => x.Price)
                   .HasColumnType("DECIMAL(9, 6)")
                   .IsRequired();
            builder.Property(x => x.DurationInDays)
                   .HasColumnType("INT")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasMany(x => x.Subscriptions)
                    .WithOne(x => x.Plan)
                    .HasForeignKey(x => x.PlanId)
                    .IsRequired();
            builder.HasMany(x => x.PlanDetails)
                    .WithOne(x => x.Plan)
                    .HasForeignKey(x => x.PlanId)
                    .IsRequired();
        }
    }
}
