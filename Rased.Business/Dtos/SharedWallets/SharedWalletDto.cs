using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.SharedWallets
{
    public class SharedWalletDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        // Relations
        [Required]
        public int CurrencyId { get; set; }
        public int ColorTypeId { get; set; }
        public int WalletStatusId { get; set; }
    }
}
