using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SubCategories;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.SubCategories
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SubCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CreateNewSubCategory(CreateSubCategoryDto model)
        {
            try
            {
                // Check the SubCategory Name
                var check = await _unitOfWork.SubCategories.CheckHelper(model.Name, 0, model.CategoryId);
                if (!check.IsSucceeded)
                {
                    return new ApiResponse<string>(check.Message!);
                }
                // Get the Parent Category from the Database
                Expression<Func<Category, bool>>[] filters = { x => x.CategoryId == model.CategoryId };
                var parentCategory = await _unitOfWork.Categories.GetData(filters).FirstOrDefaultAsync();
                // Create the SubCategory
                var subCategory = new SubCategory
                {
                    Name = model.Name,
                    Icon = model.Icon,
                    Color = model.Color,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now,
                    ParentCategoryId = model.CategoryId,
                    ParentCategory = parentCategory!
                };
                // Add the SubCategory to the Database
                await _unitOfWork.SubCategories.AddAsync(subCategory);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "SubCategory Created Successfully!");
        }

        public async Task<ApiResponse<string>> UpdateSubCategory(int id, CreateSubCategoryDto model)
        {
            try
            {
                // Get the SubCategory from the Database
                Expression<Func<SubCategory, bool>>[] filters = { x => x.SubCategoryId == id };
                var subCategory = await _unitOfWork.SubCategories.GetData(filters).FirstOrDefaultAsync();
                if (subCategory == null)
                {
                    return new ApiResponse<string>("SubCategory Not Found!");
                }

                // Check the SubCategory Name
                var check = await _unitOfWork.SubCategories.CheckHelper(model.Name, id, model.CategoryId);
                if (!check.IsSucceeded)
                {
                    return new ApiResponse<string>(check.Message!);
                }
                // Get the Parent Category from the Database
                Expression<Func<Category, bool>>[] filters2 = { x => x.CategoryId == model.CategoryId };
                var parentCategory = await _unitOfWork.Categories.GetData(filters2).FirstOrDefaultAsync();

                // Update the SubCategory
                subCategory.Name = model.Name;
                subCategory.Icon = model.Icon;
                subCategory.Color = model.Color;
                subCategory.IsActive = model.IsActive;
                subCategory.ParentCategoryId = model.CategoryId;
                subCategory.ParentCategory = parentCategory!;

                // Save the Changes to the Database
                await _unitOfWork.SubCategories.UpdateAsync(subCategory);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "SubCategory Updated Successfully!");
        }

        public async Task<ApiResponse<string>> RemoveSubCategory(int id)
        {
            try
            {
                // Get the SubCategory from the Database
                Expression<Func<SubCategory, bool>>[] filters = { x => x.SubCategoryId == id };
                var subCategory = await _unitOfWork.SubCategories.GetData(filters).FirstOrDefaultAsync();
                if (subCategory == null)
                {
                    return new ApiResponse<string>("SubCategory Not Found!");
                }

                // Remove the SubCategory from the Database
                _unitOfWork.SubCategories.Remove(subCategory);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "SubCategory Deleted Successfully!");
        }

        public async Task<ApiResponse<ReadSubCategoryDto>> GetSubCategoryById(int id)
        {
            try
            {
                // Get the SubCategory from the Database
                Expression<Func<SubCategory, bool>>[] filters = { x => x.SubCategoryId == id };
                Expression<Func<SubCategory, object>>[] includes = { x => x.ParentCategory };
                var subCategory = await _unitOfWork.SubCategories.GetData(filters, includes, false).FirstOrDefaultAsync();
                if (subCategory == null)
                {
                    return new ApiResponse<ReadSubCategoryDto>("SubCategory Not Found!");
                }
                // Map the SubCategory to ReadCategoryDto
                var readSubCategoryDto = new ReadSubCategoryDto
                {
                    Id = subCategory.SubCategoryId,
                    MainCategoryName = subCategory.ParentCategory.Name,
                    Name = subCategory.Name,
                    Icon = subCategory.Icon,
                    Color = subCategory.Color,
                    IsActive = subCategory.IsActive,
                    CreatedAt = subCategory.CreatedAt,
                    UpdatedAt = subCategory.UpdatedAt
                };

                return new ApiResponse<ReadSubCategoryDto>(readSubCategoryDto);
            }
            catch (Exception e)
            {
                return new ApiResponse<ReadSubCategoryDto>(e.Message);
            }
        }

        public async Task<string> GetSubCategoryNameById(int id)
        {
            if (1 > id)
                return string.Empty;
        
            var res = await _unitOfWork.SubCategories.GetByIdAsync(id);
            return res.Name;

        }
    }
}
