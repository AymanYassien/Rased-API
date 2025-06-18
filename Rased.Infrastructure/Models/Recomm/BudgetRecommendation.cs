using Rased.Infrastructure.Models.SharedWallets;
using Rased.Infrastructure.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Recomm
{
    public class BudgetRecommendation
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public virtual RasedUser User { get; set; } = new RasedUser();

        public int? WalletId { get; set; }
        public Wallet Wallet { get; set; }

        public int? WalletGroupId { get; set; }
        public SharedWallet WalletGroup { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } = false;


        public int Month { get; set; }
        public int Year { get; set; }
    }


}
