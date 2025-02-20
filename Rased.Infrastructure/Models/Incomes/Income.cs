using Rased.Infrastructure.Models.Wallets;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Incomes
{
    public class Income
    {
        public int IncomeId { get; set; }
        public string Tittle { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool IsAutomated { get; set; }


        [ForeignKey("Wallet")]
        public int WalletId { get; set; }
        public Wallet Wallet { get; set; }

        [ForeignKey("IncomeSource")]
        public int SourceId { get; set; }
        public IncomeSource Source { get; set; }


        [ForeignKey("IncomeTemplate")]
        public int? IncomeTemplateId { get; set; }
        public IncomeTemplate IncomeTemplate { get; set; }

        public string Status { get; set; } 



    }


    public enum IncomeStatus
    {
        PENDING,
        RECEIVED,
        DELAYED,
        PARTIAL,
        CANCELLED
    }
}
