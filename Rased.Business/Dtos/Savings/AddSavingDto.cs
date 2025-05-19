using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Savings
{
    public class AddSavingDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsSaving { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int? SubCatId { get; set; }
    }
}
