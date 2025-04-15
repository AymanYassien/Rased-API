using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.SubCategories
{
    public class SubCategoryRepository: Repository<SubCategory, int>, ISubCategoryRepository
    {
        public SubCategoryRepository(RasedDbContext context) : base(context)
        {
        }

        public async Task<StatusDto> CheckHelper(string name, int catId, int parentCatId)
        {
            var result = new StatusDto();
            
            // Check if the name is empty
            if (string.IsNullOrEmpty(name))
            {
                result.Message = "SubCategory Name is required!";
                return result;
            }
            // Check if the parent category exists
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == parentCatId);
            if (category == null)
            {
                result.Message = "Parent Category Not Found!";
                return result;
            }
            // if catId <= 0 so it is check for adding
            if (catId <= 0)
            {
                var cat = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
                if (cat != null)
                {
                    result.Message = "SubCategory Name already exists, Try Another One!";
                    return result;
                }
                result.IsSucceeded = true;
            }
            // if catId > 0 so it is check for updating
            else
            {
                var oldCategory = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.SubCategoryId == catId);
                if (oldCategory == null)
                {
                    result.Message = "SubCategory Not Found!";
                    return result;
                }
                var cat = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name && oldCategory.Name != name);
                if (cat != null)
                {
                    result.Message = "SubCategory Name already exists, Try Another One!";
                    return result;
                }
                result.IsSucceeded = true;
            }

            return result;
        }
    }
}
