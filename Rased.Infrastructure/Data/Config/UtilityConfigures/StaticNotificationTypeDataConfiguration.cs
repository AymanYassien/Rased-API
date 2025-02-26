using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure;

namespace Rased.Business
{
    public class StaticNotificationTypeDataConfiguration : IEntityTypeConfiguration<StaticNotificationTypeData>
{
    public void Configure(EntityTypeBuilder<StaticNotificationTypeData> entity)
    {
        entity.HasKey(e => e.Id);

        entity.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        entity.Property(e => e.NameInArabic)
            .IsRequired()
            .HasMaxLength(50)
            .HasColumnType("nvarchar(50)");

        entity.Property(e => e.Message)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        entity.Property(e => e.MessageInArabic)
            .IsRequired()
            .HasMaxLength(200)
            .HasColumnType("nvarchar(200)");

        entity.HasData(
            new StaticNotificationTypeData
            {
                Id = 1,
                Name = "EXPENSE_LIMIT_REACHED",
                NameInArabic = "تجاوزت حد الإنفاق",
                Message = "Your expense limit has been exceeded.",
                MessageInArabic = "لقد تجاوزت حد الإنفاق الخاص بك."
            },
            new StaticNotificationTypeData
            {
                Id = 2,
                Name = "EXPENSE_LIMIT_APPROACHING",
                NameInArabic = "اقتراب حد الإنفاق",
                Message = "You are approaching your expense limit.",
                MessageInArabic = "أنت تقترب من حد الإنفاق الخاص بك."
            },
            new StaticNotificationTypeData
            {
                Id = 3,
                Name = "GOAL_ACHIEVED",
                NameInArabic = "تم تحقيق الهدف المالي",
                Message = "Congratulations! You have achieved your financial goal.",
                MessageInArabic = "تهانينا! لقد حققت هدفك المالي."
            },
            new StaticNotificationTypeData
            {
                Id = 4,
                Name = "GOAL_MILESTONE",
                NameInArabic = "الوصول إلى نقطة معينة في الهدف",
                Message = "You have reached a milestone in your goal.",
                MessageInArabic = "لقد وصلت إلى نقطة مهمة في هدفك."
            },
            new StaticNotificationTypeData
            {
                Id = 5,
                Name = "LOW_BALANCE",
                NameInArabic = "انخفاض الرصيد",
                Message = "Your wallet balance is running low.",
                MessageInArabic = "رصيد محفظتك ينخفض."
            },
            new StaticNotificationTypeData
            {
                Id = 6,
                Name = "LOAN_PAYMENT_DUE",
                NameInArabic = "موعد سداد القرض",
                Message = "Your loan payment is due soon.",
                MessageInArabic = "موعد سداد قرضك قريب."
            },
            new StaticNotificationTypeData
            {
                Id = 7,
                Name = "BUDGET_EXCEEDED",
                NameInArabic = "تجاوزت الميزانية",
                Message = "You have exceeded your budget.",
                MessageInArabic = "لقد تجاوزت ميزانيتك."
            }
        );
    }
}
}

