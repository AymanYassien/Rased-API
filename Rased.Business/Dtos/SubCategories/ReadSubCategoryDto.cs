using Rased.Business.Dtos.Categories;

namespace Rased.Business.Dtos.SubCategories
{
    public class ReadSubCategoryDto: CategoryDto
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? MainCategoryName { get; set; }
    }
}
