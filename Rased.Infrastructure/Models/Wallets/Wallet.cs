using Rased.Infrastructure.Models.Incomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Rased.Infrastructure.Models.Wallets
{
    public class Wallet
    {
        public int WalletId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastModified { get; set; }

        public string WalletStatus { get; set; }
        public string ColorType { get; set; }
        public string CurrencyType { get; set; }


        public virtual ICollection<Income> Incomes { get; set; } = new HashSet<Income>();
        //public virtual ICollection<Expense> Expenses { get; set; } = new HashSet<Expense>();
        //public virtual ICollection<Budget> Budgets { get; set; } = new HashSet<Budget>();
        //public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
        //public virtual ICollection<Goal> Goals { get; set; } = new HashSet<Goal>();
        //public virtual ICollection<Saving> Savings { get; set; } = new HashSet<Saving>();



    }
    public enum WalletStatus
    {
        ACTIVE,
        ARCHIVED,
        DELETED
    }
    public enum ColorType
    {
        Black,
        White,
        Custom
    }
    //public enum CurrencyType
    //{
    //    Dollar,
    //    SAR,       // Saudi Riyal
    //    EGP,       // Egyptian Pound
    //    QAR,       // Qatari Riyal
    //    KWD,       // Kuwaiti Dinar
    //    AED        // UAE Dirham
    //}



}
