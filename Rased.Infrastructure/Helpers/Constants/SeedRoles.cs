using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rased.Infrastructure.Helpers.Constants
{
    public class SeedRoles
    {
        public static async Task SeedRole(RoleManager<CustomRole> roleManager)
        {
            var roles = new List<string> { RasedRolesConstants.User , RasedRolesConstants.SuperAdmin , RasedRolesConstants.Admin };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new CustomRole { Name = role });
                }
            }
        }
    }

}
