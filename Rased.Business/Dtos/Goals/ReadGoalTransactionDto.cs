using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Business.Dtos.Goals
{
    public class ReadGoalTransactionDto
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public decimal InsertedAmount { get; set; }
        public DateTime InsertedDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
