using Rased.Business.Dtos.Categories;
using Rased.Business.Dtos.Response;

namespace Rased.Business.Services.Categories
{
    public interface ICategoryService
    {
        // Your Services
        // Add New Category
        Task<ApiResponse<string>> CreateNewCategory(CategoryDto model);
        // Update Category
        Task<ApiResponse<string>> UpdateCategory(int id, CategoryDto model);
        // Delete Category
        Task<ApiResponse<string>> RemoveCategory(int id);
        // Read All Categories with its Subs
        Task<ApiResponse<List<ReadCategoryDto>>> GetAllCategories();
        // Read Category by Id
        Task<ApiResponse<ReadCategoryDto>> GetCategoryById(int id);
    }
}
