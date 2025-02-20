namespace Rased.Infrastructure.Models.Preference
{
    public class NotificationSetting
    {
        public int Id { get; set; }
        public string Language { set; get; } = null!; 
        public bool EnableEmails { get; set; }
        public bool EnableAll { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int UserPrefId { get; set; }

        // Navigation Properties
        public UserPreference UserPreference { get; set; } = new UserPreference();
    }
}
