using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;

namespace Rased.Infrastructure
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Category ParentCategory { get; set; } = new Category();
        public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual ICollection<ExpenseTemplate> ExpenseTemplates { get; set; } = new List<ExpenseTemplate>();
        //public virtual ICollection<Income> Incomes { get; set; } = new List<Income>();
        public virtual ICollection<IncomeTemplate> IncomeTemplates { get; set; } = new List<IncomeTemplate>();
    }
}