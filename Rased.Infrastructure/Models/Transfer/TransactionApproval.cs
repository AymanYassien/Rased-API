
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure
{
    // Approval Transaction from person or shared wallet
    public class TransactionApproval
    {
        public int ApprovalId { get; set; }
        public int TransactionId { get; set; }
        public string ApproverId { get; set; } //   Friend Or Shared Wallet 
        public DateTime ApprovedAt { get; set; }  

        // Navigation properties
        public virtual Transaction Transaction { get; set; }
        public virtual RasedUser Approver { get; set; }
    }

}