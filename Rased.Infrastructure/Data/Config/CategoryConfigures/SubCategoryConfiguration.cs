
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;


namespace Rased.Business
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategory>
    {
        public void Configure(EntityTypeBuilder<SubCategory> builder)
        {
            builder.HasKey(sc => sc.SubCategoryId);

            builder.Property(sc => sc.ParentCategoryId)
                .IsRequired();

            builder.Property(sc => sc.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(true);

            builder.Property(sc => sc.Icon)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(sc => sc.IsActive)
                .HasDefaultValue(true);

            builder.HasOne(sc => sc.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(sc => sc.ParentCategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}