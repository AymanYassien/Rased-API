using Microsoft.AspNetCore.Identity;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;

namespace Rased.Infrastructure.Repositoryies.Friendships
{
    public class FriendshipRepository: Repository<Friendship, int>, IFriendshipRepository
    {
        private readonly UserManager<RasedUser> _userManager;

        public FriendshipRepository(RasedDbContext context, UserManager<RasedUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        // Get the user Id by its email
        public async Task<string> GetUserIdByEmailAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user is null)
                    return string.Empty;

                return user.Id;
            }
            catch
            {
                return string.Empty;
            }
        }

        // Get User By Id
        public async Task<RasedUser> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                    return null!;

                return user;
            }
            catch
            {
                return null!;
            }
        }
    }
}
