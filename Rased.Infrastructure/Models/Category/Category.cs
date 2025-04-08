
namespace Rased.Infrastructure
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //public int CategoryTypeId { get; set; }

        public bool IsActive { get; set; }

        // Navigation property
        public virtual ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
        //public virtual StaticCategoryTypesData StaticCategoryTypesData { get; set; }
    }
}
