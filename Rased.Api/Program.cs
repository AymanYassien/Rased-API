
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rased.Business;
using Rased.Business.Services.AuthServices;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.UnitsOfWork;

namespace Rased.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // Add RasedDbContext Service To The Pipeline
            builder.Services.AddDbContext<RasedDbContext>(
                op => op.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConStr"),
                    con => con.MigrationsAssembly(typeof(RasedDbContext).Assembly.FullName)
                )
            );


            // System Services Registeration

            // Register Identity
            builder.Services.AddIdentity<RasedUser, IdentityRole>().AddEntityFrameworkStores<RasedDbContext>();

            // Register Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Register Business Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // Register Repository 
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));




            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
