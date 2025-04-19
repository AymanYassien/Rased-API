using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased.Infrastructure.Repositoryies.Friendships
{
    public interface IFriendshipRepository : IRepository<Friendship, int>
    {
        Task<string> GetUserIdByEmailAsync(string email);
        Task<RasedUser> GetUserByIdAsync(string id);
    }
}
