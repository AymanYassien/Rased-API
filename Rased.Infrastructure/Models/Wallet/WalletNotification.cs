using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure
{
    public class WalletNotification
    {
        public int WalletNotificationId { get; set; }
        public int WalletId { get; set; }
        public int NotificationTypeId { get; set; }
        public int ThresholdTypeId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;
        public decimal Threshold { get; set; }
        public decimal? CurrentAmount { get; set; }
        
        public virtual Wallet Wallet { get; set; }
        public virtual StaticNotificationTypeData NotificationType { get; set; }
        public virtual StaticThresholdTypeData ThresholdType { get; set; }
    }
   
}
