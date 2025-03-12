using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Goals
{
    public class UpdateGoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }   // C(Completed) - P(Progressing)
        public DateTime StartedDate { get; set; }
        public DateTime DesiredDate { get; set; }
        public decimal StartedAmount { get; set; }
        public decimal TargetAmount { get; set; }

        public bool IsTemplate { get; set; }
        public string? Frequency { get; set; }
        public decimal? FrequencyAmount { get; set; }

        // Parent Ids
        //public int? WalletId { get; set; }
        //public int? SharedWalletId { get; set; }
        //public int? SubCatId { get; set; }
    }

}
