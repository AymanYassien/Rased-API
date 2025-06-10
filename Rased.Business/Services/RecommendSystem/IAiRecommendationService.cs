using Rased.Business.Dtos.Recomm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Services.RecommendSystem
{
    public interface IAiRecommendationService
    {
        Task<WalletDataForAI> CollectWalletDataAsync(int walletId, string userId);
    }
}
