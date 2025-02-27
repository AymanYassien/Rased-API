namespace Rased.Infrastructure
{
    public class SubCategory
    {
        public int SubCategoryId { get; set; }
        public int ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public bool IsActive { get; set; }

        // Navigation properties
        public virtual Category ParentCategory { get; set; }

    }
}