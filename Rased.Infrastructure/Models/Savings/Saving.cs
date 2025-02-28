namespace Rased.Infrastructure.Models.Savings
{
    public class Saving
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsSaving { get; set; }     // 1 - 0
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Parents Ids
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int? SubCatId { get; set; }

        // Navigation Properties
        public Wallet? Wallet { get; set; }
        public SharedWallet? SharedWallet { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
