﻿using Rased.Infrastructure.Models.Goals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Goals
{
    public class ReadGoalDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public GoalStatusEnum Status { get; set; } 
        public DateTime StartedDate { get; set; } 
        public DateTime DesiredDate { get; set; }
        public decimal StartedAmount { get; set; } 
        public decimal CurrentAmount { get; set; }
        public decimal TargetAmount { get; set; }
      
        public string? SubCategoryName { get; set; }

        //public bool IsTemplate { get; set; } = false;
        //public string? Frequency { get; set; }
        //public decimal? FrequencyAmount { get; set; }

        // Parent Ids
        public int? WalletId { get; set; }
        public int? SharedWalletId { get; set; }
        public int? SubCatId { get; set; }
    }

}
