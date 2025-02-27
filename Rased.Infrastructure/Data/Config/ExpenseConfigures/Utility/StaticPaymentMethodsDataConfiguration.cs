using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business;

public class StaticPaymentMethodsDataConfiguration : IEntityTypeConfiguration<StaticPaymentMethodsData>
{
    public void Configure(EntityTypeBuilder<StaticPaymentMethodsData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(20); 
        
        /*entity.HasData(
            new StaticPaymentMethodsData
            {
                Id = 1,
                Name = "CASH"
            },
            new StaticPaymentMethodsData
            {
                Id = 2,
                Name = "CREDIT_CARD"
            },
            new StaticPaymentMethodsData
            {
                Id = 3,
                Name = "DEBIT_CARD" 
            },
            new StaticPaymentMethodsData
            {
                Id = 4,
                Name = "MOBILE_PAYMENT"
            },
            new StaticPaymentMethodsData
            {
                Id = 5,
                Name = "CRYPTO"
            },
            new StaticPaymentMethodsData
            {
                Id = 6,
                Name = "BANK_TRANSFER"
            },
            new StaticPaymentMethodsData
            {
                Id = 7,
                Name = "OTHER"
            }
        );*/
    }
}