using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Rased.Infrastructure.Models.Wallets;

namespace Rased.Infrastructure.Models
{
    public class AutomationRule
    {
       
        public int AutomationRuleId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? DayOfMonth { get; set; } 
        public int? DayOfWeek { get; set; }

        [Required]
        public string TriggerType { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

    }
    public enum TriggerType
    {
        MONTHLY_ON_DAY,       // يوم معين في كل شهر
        WEEKLY_ON_DAY,        // يوم معين في كل أسبوع
        MONTHLY_ON_LAST_DAY,  // آخر يوم في كل شهر
        MONTHLY_ON_FIRST_DAY, // أول يوم في كل شهر
        CUSTOM_INTERVAL       // فاصل زمني مخصص
    }

}
