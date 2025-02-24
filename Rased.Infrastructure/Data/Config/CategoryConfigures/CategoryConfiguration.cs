
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Categories;



namespace Rased.Infrastructure.Data.Config.CategoryConfigures
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(true);
            // .HasColumnType("nvarchar(50)");

            builder.Property(c => c.Icon)
                .HasMaxLength(100)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(c => c.Color)
                .HasMaxLength(20)
                .IsUnicode(true)
                .IsRequired(false);

            builder.Property(c => c.Type)
                .IsRequired();

            builder.Property(c => c.IsActive)
                .HasDefaultValue(true);
        }

    }
}