using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticReceiverTypeDataConfiguration : IEntityTypeConfiguration<StaticReceiverTypeData>
{
    public void Configure(EntityTypeBuilder<StaticReceiverTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
       .IsRequired()
       .HasMaxLength(100); // ⁄œ· «·—ﬁ„ Â‰« Õ”» «·Õ«Ã…



     
    }
}