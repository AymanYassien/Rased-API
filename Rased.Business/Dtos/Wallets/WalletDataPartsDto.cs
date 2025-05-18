using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Wallets
{
    public class WalletDataPartsDto
    {
        public List<WalletStatus>? Status { get; set; }
        public List<WalletColor>? Color { get; set; }
        public List<WalletCurrency>? Currency { get; set; }
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
