using Rased.Infrastructure.Models.SharedWallets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Bills
{
    public class BillDraft
    {
        public int BillDraftId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; } 
        public string? Description { get; set; }

        // علاقة "واحد إلى متعدد" مع المرفقات
        public virtual Wallet? Wallet { get; set; }
        public virtual SharedWallet? SharedWallet { get; set; }
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

}
