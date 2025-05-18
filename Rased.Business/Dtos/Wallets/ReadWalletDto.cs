namespace Rased.Business.Dtos.Wallets
{
    public class ReadWalletDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Relations
        public WalletColor? WalletColor { get; set; }
        public WalletCurrency? WalletCurrency { get; set; }
        public WalletStatus? WalletStatus { get; set; }
    }
}
