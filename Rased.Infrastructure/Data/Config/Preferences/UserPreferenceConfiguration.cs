using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rased.Infrastructure.Models.Preferences;

namespace Rased.Infrastructure.Data.Config.Preferences
{
    public class UserPreferenceConfiguration : IEntityTypeConfiguration<UserPreference>
    {
        public void Configure(EntityTypeBuilder<UserPreference> builder)
        {
            builder.ToTable("UserPreferences");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();
            builder.Property(x => x.Language)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.Theme)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.DateFormat)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.MoneyFormat)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.TimeZone)
                   .HasColumnType("NVARCHAR(255)")
                   .IsRequired();
            builder.Property(x => x.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
            builder.Property(x => x.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false);

            // Relationships
            builder.HasOne(x => x.User)
                   .WithOne(x => x.Preference)
                   .HasForeignKey<UserPreference>(x => x.UserId)
                   .IsRequired();
            builder.HasOne(x => x.Currency)
                    .WithMany(x => x.Preferences)
                    .HasForeignKey(x => x.CurrencyId)
                    .IsRequired();
            builder.HasOne(x => x.NotificationSetting)
                   .WithOne(x => x.Preference)
                   .HasForeignKey<NotificationSetting>(x => x.UserPrefId)
                   .IsRequired();
        }
    }
}
