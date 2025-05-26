using Rased.Business.Dtos.Wallets;

namespace Rased.Business.Dtos.SharedWallets
{
    public class ReadSharedWalletDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public decimal InitialBalance { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal ExpenseLimit { get; set; }
        public bool IsOwner { get; set; } = false; // Indicates if the current user is the owner of the shared wallet
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Relations
        public List<SWMembersDto>? Members { get; set; }
        public WalletStatus? WalletStatus { get; set; }
        public WalletColor? WalletColor { get; set; }
        public WalletCurrency? WalletCurrency { get; set; }
    }

    public class SWMembersDto
    {
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
