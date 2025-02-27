


using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rased.Infrastructure
{
    public class BudgetAlert
    {
        public int AlertId { get; set; }
        public int BudgetId { get; set; }
        public decimal Threshold { get; set; }
        public StaticThresholdTypeData Type { get; set; }
        public StaticNotificationTypeData NotificationType { get; set; }

        public bool IsEnabled { get; set; }

        // Navigation property
        public virtual Budget Budget { get; set; }
    }
}

