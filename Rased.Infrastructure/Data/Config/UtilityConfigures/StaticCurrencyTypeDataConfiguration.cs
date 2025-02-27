using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rased.Business;

public class StaticCurrencyTypeDataConfiguration : IEntityTypeConfiguration<StaticCurrencyTypeData>
{
    public void Configure(EntityTypeBuilder<StaticCurrencyTypeData> entity) 
    {
        entity.HasKey(e => e.id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");
        
        entity.Property(e => e.Code)
            .IsRequired()
            .HasMaxLength(10);
        
        entity.Property(e => e.Icon)
            .IsRequired(false)
            .HasMaxLength(100);
        
        entity.Property(e => e.ValueOverDollar)
            .IsRequired(true)
            .HasColumnType("decimal(5,3)")
            .HasPrecision(5,3);
        
        
        entity.HasData(
            new StaticCurrencyTypeData 
            { 
                id = 1, 
                Name = "United States Dollar", 
                Code = "USD", 
                ValueOverDollar = 1.000m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 2, 
                Name = "Euro", 
                Code = "EUR", 
                ValueOverDollar = 1.080m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 3, 
                Name = "Egyptian Pound", 
                Code = "EGP", 
                ValueOverDollar = 0.020m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 4, 
                Name = "Saudi Riyal", 
                Code = "SAR", 
                ValueOverDollar = 0.267m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 5, 
                Name = "Kuwaiti Dinar", 
                Code = "KWD", 
                ValueOverDollar = 3.260m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 6, 
                Name = "United Arab Emirates Dirham", 
                Code = "AED", 
                ValueOverDollar = 0.272m 
            },
            new StaticCurrencyTypeData 
            { 
                id = 7, 
                Name = "Qatari Riyal", 
                Code = "QAR", 
                ValueOverDollar = 0.275m 
            }
        );
        
    }
}
