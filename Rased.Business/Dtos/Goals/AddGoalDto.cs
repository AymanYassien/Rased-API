using Rased.Infrastructure.Models.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Goals
{
    public class AddGoalDto
    {
        public string Name { get; set; } = string.Empty;
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public GoalStatusEnum Status { get; set; } = GoalStatusEnum.InProgress;  // C(Completed) - P(Progressing)
        public DateTime StartedDate { get; set; } = DateTime.UtcNow;
        public DateTime DesiredDate { get; set; }
        public decimal StartedAmount { get; set; } = 0;
        public decimal TargetAmount { get; set; }

        //public bool IsTemplate { get; set; } = false;
        //public string? Frequency { get; set; }
        //public decimal? FrequencyAmount { get; set; }

        // Parent Ids
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int? SubCatId { get; set; }
    }

}
