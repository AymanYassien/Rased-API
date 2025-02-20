using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Incomes
{
    public class IncomeTemplate
    {

        public int IncomeTemplateId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        [ForeignKey("IncomeSource")]
        public int SourceId { get; set; }
        public IncomeSource Source { get; set; }

        //[ForeignKey("PaymentMethod")]
        //public int DefaultPaymentId { get; set; }
        //public PaymentMethod DefaultPayment { get; set; }

        [ForeignKey("AutomationRule")]
        public int? AutomationRuleId { get; set; }
        public AutomationRule AutomationRule { get; set; }
    }
}
