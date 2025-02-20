using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Wallets
{
    public class WalletNotification
    {

        public int WalletNotificationId { get; set; }

        public string Type { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public decimal? Threshold { get; set; }
        public decimal? CurrentAmount { get; set; }

        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

    }
    public enum NotificationType
    {
        EXPENSE_LIMIT_REACHED,      // تم تجاوز حد الإنفاق
        EXPENSE_LIMIT_APPROACHING,  // اقتراب حد الإنفاق
        GOAL_ACHIEVED,             // تم تحقيق الهدف المالي
        GOAL_MILESTONE,            // الوصول إلى نقطة معينة في الهدف
        LOW_BALANCE,               // انخفاض الرصيد
        LOAN_PAYMENT_DUE,          // موعد سداد القرض
        BUDGET_EXCEEDED            // تم تجاوز الميزانية
    }
}
