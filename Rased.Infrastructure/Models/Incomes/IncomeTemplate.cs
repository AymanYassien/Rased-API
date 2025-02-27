using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure
{
    public class IncomeTemplate
    {

        public int IncomeTemplateId { get; set; }
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int AutomationRuleId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int IncomeSourceTypeId { get; set; }
        public decimal Amount { get; set; }
        public bool IsNeedApprovalWhenAutoAdd { get; set; }
        
        public virtual Wallet Wallet { get; set; }
        public virtual SharedWallet SharedWallet { get; set; } 
        public virtual AutomationRule AutomationRule { get; set; } 
        public virtual StaticIncomeSourceTypeData IncomeSourceType { get; set; }
        // public virtual ICollection<Income> Incomes { get; set; } 
        
    }
}
