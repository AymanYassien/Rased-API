
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{
    public class TransactionApproval
    {
        public int ApprovalId { get; set; }
        public int TransactionId { get; set; }
        public int ApproverId { get; set; }
        public DateTime ApprovedAt { get; set; }

        // Navigation properties
        public virtual Transaction Transaction { get; set; }
        public virtual RasedUser Approver { get; set; }
    }

}