using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Models.Incomes
{
    public class IncomeSource
    {

        public int IncomeSourceId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }

        public bool IsRecurring { get; set; }

        public string Type { get; set; }

    }
    public enum SourceType
    {
        SALARY,
        FREELANCE,
        INVESTMENT,
        RENTAL,
        BUSINESS,
        OTHER
    }
}
