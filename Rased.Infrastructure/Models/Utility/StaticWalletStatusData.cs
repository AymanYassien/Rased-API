
namespace Rased.Infrastructure
{
    public class StaticWalletStatusData
    {
        public int id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Wallet>? Wallets { get; set; }
    } 
}