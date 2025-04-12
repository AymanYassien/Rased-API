using Rased.Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Transfer
{
    public class TransactionRejection
    {
        public int RejectionId { get; set; }
        public int TransactionId { get; set; }
        public string RejectedById { get; set; } 
        public DateTime RejectedAt { get; set; }
        public string? RejectionReason { get; set; }
   

        // Navigation properties
        public virtual Transaction Transaction { get; set; }
        public virtual RasedUser RejectedBy { get; set; }
    }

}
