using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rased.Infrastructure.Data.Config.Savings;
using Rased.Infrastructure.Models.User;

namespace Rased.Infrastructure.Data
{
    public class RasedDbContext: IdentityDbContext<RasedUser>
    {
        public RasedDbContext(DbContextOptions<RasedDbContext> options) : base(options) { }

        // DbSets ...
        // ...


        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // This is for Saving Folder
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SavingConfiguration).Assembly);
            // and so on ......
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(WalletConfiguration).Assembly);
        }
    }
}
