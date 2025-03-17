namespace Rased.Infrastructure;

public class StaticColorTypeData
{
        public int id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Wallet>? Wallets { get; set; }
}
