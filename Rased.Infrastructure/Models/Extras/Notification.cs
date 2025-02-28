using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Models.Extras
{
    public class Notification
    {
        public int Id { get; set; }
        public string TitleEn { get; set; }
        public string TitleAr { get; set; }
        public string MessageEn { get; set; }
        public string MessageAr { get; set; }
        public string url { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // ParentIds
        public string UserId { get; set; }

        // Navigation Properties
        public RasedUser User { get; set; } = new RasedUser();
    }
}
