namespace Rased.Infrastructure.Models.Preference
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public IEnumerable<UserPreference> UserPreferences { get; set; } = new List<UserPreference>();
    }
}
