using Microsoft.AspNetCore.Identity;

namespace Rased.Infrastructure.Helpers.Constants
{
    public static class RasedRolesConstants
    {
        public const string SuperAdmin = "SuperAdmin";
        public const string Admin = "Admin";
        public const string User = "User";
    }



    public class CustomRole : IdentityRole
    {
        public bool IsDeleted { get; set; } = false;


    }
}
