namespace Rased.Infrastructure.Repositoryies.DTOs
{
    public class WalletDataPartsDto
    {
        public WalletColor Color { get; set; } = new();
        public WalletStatus Status { get; set; } = new();
        public WalletCurrency Currency { get; set; } = new();
    }

    public class WalletCurrency
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class WalletColor
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class WalletStatus
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
