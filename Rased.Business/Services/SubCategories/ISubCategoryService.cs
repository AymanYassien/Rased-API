using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SubCategories;

namespace Rased.Business.Services.SubCategories
{
    public interface ISubCategoryService
    {
        // Add New SubCategory
        Task<ApiResponse<string>> CreateNewSubCategory(CreateSubCategoryDto model);
        // Update SubCategory
        Task<ApiResponse<string>> UpdateSubCategory(int id, CreateSubCategoryDto model);
        // Delete Category
        Task<ApiResponse<string>> RemoveSubCategory(int id);
        // Read SubCategory by Id
        Task<ApiResponse<ReadSubCategoryDto>> GetSubCategoryById(int id);
    }
}
