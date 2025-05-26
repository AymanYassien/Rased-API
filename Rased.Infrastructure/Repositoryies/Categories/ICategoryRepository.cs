using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.Categories
{
    public interface ICategoryRepository: IRepository<Category, int>
    {
        Task<StatusDto> CheckHelper(string name, int catId);
        public Task<int> GetCategoryIdByName(string name);
    }
}
