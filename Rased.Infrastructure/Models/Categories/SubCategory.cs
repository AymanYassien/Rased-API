using Rased.Infrastructure.Models.Budgets;
using Rased.Infrastructure.Models.Expenses;
using Rased.Infrastructure.Models.Goals;
using Rased.Infrastructure.Models.Savings;

namespace Rased.Infrastructure.Models.Categories
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Category Category { get; set; } = new Category();
        public virtual ICollection<Saving> Savings { get; set; } = new List<Saving>();
        public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();
        public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();
        public virtual ICollection<Expense> Expenses { get; set; } = new List<Expense>();
        public virtual ICollection<ExpenseTemplate> ExpenseTemplates { get; set; } = new List<ExpenseTemplate>();
    }
}