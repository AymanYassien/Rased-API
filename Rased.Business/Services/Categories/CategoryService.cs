using Microsoft.EntityFrameworkCore;
using Rased.Business.Dtos.Categories;
using Rased.Business.Dtos.Response;
using Rased.Business.Dtos.SubCategories;
using Rased.Infrastructure;
using Rased.Infrastructure.UnitsOfWork;
using System.Linq.Expressions;

namespace Rased.Business.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<string>> CreateNewCategory(CategoryDto model)
        {
            try
            {
                // Check the Category Name
                var check = await _unitOfWork.Categories.CheckHelper(model.Name, 0);
                if (!check.IsSucceeded)
                {
                    return new ApiResponse<string>(check.Message!);
                }

                // Create the Category
                var category = new Category
                {
                    Name = model.Name,
                    Icon = model.Icon,
                    Color = model.Color,
                    IsActive = model.IsActive,
                    CreatedAt = DateTime.Now
                };

                // Add the Category to the Database
                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Category Created Successfully!");
        }

        public async Task<ApiResponse<string>> UpdateCategory(int id, CategoryDto model)
        {
            try
            {
                // Check the Category Name
                var check = await _unitOfWork.Categories.CheckHelper(model.Name, id);
                if (!check.IsSucceeded)
                {
                    return new ApiResponse<string>(check.Message!);
                }

                // Get the current category
                Expression<Func<Category, bool>>[] filters = { x => x.CategoryId == id };
                var currentCategory = await _unitOfWork.Categories.GetData(filters).FirstOrDefaultAsync();
                if (currentCategory == null)
                {
                    return new ApiResponse<string>("Category Not Found!");
                }

                // Update the Category
                currentCategory.Name = model.Name;
                currentCategory.Icon = model.Icon;
                currentCategory.Color = model.Color;
                currentCategory.IsActive = model.IsActive;
                currentCategory.UpdatedAt = DateTime.Now;

                // Add the Category to the Database
                await _unitOfWork.Categories.UpdateAsync(currentCategory);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Category Updated Successfully!");
        }

        public async Task<ApiResponse<string>> RemoveCategory(int id)
        {
            try
            {
                // Get the current category
                Expression<Func<Category, bool>>[] filters = { x => x.CategoryId == id };
                var currentCategory = await _unitOfWork.Categories.GetData(filters).FirstOrDefaultAsync();
                if (currentCategory == null)
                {
                    return new ApiResponse<string>("Category Not Found!");
                }
                // Remove the Category from the Database
                _unitOfWork.Categories.Remove(currentCategory);
                await _unitOfWork.CommitChangesAsync();
            }
            catch (Exception e)
            {
                return new ApiResponse<string>(e.Message);
            }

            return new ApiResponse<string>(null, "Category Removed Successfully!");
        }

        public async Task<ApiResponse<List<ReadCategoryDto>>> GetAllCategories()
        {
            var result = new List<ReadCategoryDto>();

            try
            {
                // Get All Categories
                Expression<Func<Category, object>>[] includes = { x => x.SubCategories };
                var categories = await _unitOfWork.Categories.GetData(null, includes, false).ToListAsync();
                // Check if there are no categories
                if (!categories.Any())
                    return new ApiResponse<List<ReadCategoryDto>>("No Categories Found");

                // Mapping Categories
                foreach (var category in categories)
                {
                    result.Add(new ReadCategoryDto()
                    {
                        Id = category.CategoryId,
                        Name = category.Name,
                        Icon = category.Icon,
                        Color = category.Color,
                        IsActive = category.IsActive,
                        CreatedAt = category.CreatedAt,
                        UpdatedAt = category.UpdatedAt,
                        SubCategories = category.SubCategories.Select(x => new ReadSubCategoryDto()
                        {
                            Id = x.SubCategoryId,
                            MainCategoryName = category.Name,
                            Name = x.Name,
                            Icon = x.Icon,
                            Color = x.Color,
                            IsActive = x.IsActive,
                            CreatedAt = x.CreatedAt,
                            UpdatedAt = x.UpdatedAt
                        }).ToList()
                    });
                }
            }
            catch(Exception e)
            {
                return new ApiResponse<List<ReadCategoryDto>>(e.Message);
            }

            return new ApiResponse<List<ReadCategoryDto>>(result);
        }
    }
}
