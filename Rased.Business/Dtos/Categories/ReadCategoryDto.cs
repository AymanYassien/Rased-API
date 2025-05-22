using Rased.Business.Dtos.SubCategories;

namespace Rased.Business.Dtos.Categories
{
    public class ReadCategoryDto: CategoryDto
    {
        public int Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        // Subs
        public List<ReadSubCategoryDto>? SubCategories { get; set; }
    }

    public class CatSubDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<SubCatDto> Subs { get; set; }
    }

    public class SubCatDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }
}
