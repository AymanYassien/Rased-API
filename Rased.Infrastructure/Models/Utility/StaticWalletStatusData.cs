
using Rased.Infrastructure.Models.SharedWallets;

namespace Rased.Infrastructure
{
    public class StaticWalletStatusData
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Wallet>? Wallets { get; set; }
        public virtual ICollection<SharedWallet>? SharedWallets { get; set; }
    } 
}