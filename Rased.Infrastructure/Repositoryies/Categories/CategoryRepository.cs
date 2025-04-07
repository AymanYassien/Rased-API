using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.Categories
{
    public class CategoryRepository: Repository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(RasedDbContext context) : base(context)
        {
        }

        public async Task<StatusDto> CheckHelper(string name, int catId)
        {
            var result = new StatusDto();

            // Check if the name is empty
            if (string.IsNullOrEmpty(name))
            {
                result.Message = "Category Name is required!";
                return result;
            }
            // if catId <= 0 so it is check for adding
            if (catId <= 0)
            {
                var cat = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name);
                if(cat != null)
                {
                    result.Message = "Category Name already exists, Try Another One!";
                    return result;
                }

                result.IsSucceeded = true;
            }
            // if catId > 0 so it is check for updating
            else
            {
                var oldCategory = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.CategoryId == catId);
                if(oldCategory == null)
                {
                    result.Message = "Category Not Found!";
                    return result;
                }
                var cat = await _dbSet.AsNoTracking().FirstOrDefaultAsync(c => c.Name == name && oldCategory.Name != name);
                if (cat != null)
                {
                    result.Message = "Category Name already exists, Try Another One!";
                    return result;
                }

                result.IsSucceeded = true;
            }

            return result;
        }
    }
}
