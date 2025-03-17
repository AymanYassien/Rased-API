using System.ComponentModel.DataAnnotations;

namespace Rased.Business.Dtos.Wallets
{
    public class RequestWalletDto
    {
        [Required]
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        // Relations
        [Required]
        public string CreatorId { get; set; } = null!;
        public int CurrencyId { get; set; }
        public int ColorTypeId { get; set; }
        public int WalletStatusId { get; set; }
    }
}
