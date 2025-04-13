
using Rased.Infrastructure.Models.SharedWallets;

namespace Rased.Infrastructure.Models.Extras
{
    public class Currency
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Wallet> Wallets { get; set; } = new List<Wallet>();
        public ICollection<SharedWallet> SharedWallets { get; set; } = new List<SharedWallet>();
    }
}
