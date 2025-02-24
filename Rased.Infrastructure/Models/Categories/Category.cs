namespace Rased.Infrastructure.Models.Categories
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public string? Type { get; set; }

        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
