namespace Rased.Infrastructure.Models.Preference
{
    public class UserPreference
    {
        public int Id { get; set; }
        public string Language { get; set; } = null!; // En - Ar
        public string Theme { get; set; } = null!; // Light - Dark
        public string DateFormat { get; set; } = null!;
        public string MoneyFormat { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public string UserId { get; set; } = null!;
        public int CurrencyId { get; set; }

        // Navigation Properties
        //public IdentityUser User { get; set; } = new IdentityUser();
        public Currency Currency { get; set; } = new Currency();
        public NotificationSetting NotificationSetting { get; set; } = new NotificationSetting();
    }
}
