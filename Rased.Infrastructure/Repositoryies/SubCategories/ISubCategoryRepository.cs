using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.Repositoryies.DTOs;

namespace Rased.Infrastructure.Repositoryies.SubCategories
{
    public interface ISubCategoryRepository: IRepository<SubCategory, int>
    {
        Task<StatusDto> CheckHelper(string name, int catId, int parentCatId);
    }
}
