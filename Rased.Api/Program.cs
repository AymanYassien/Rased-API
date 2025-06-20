using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rased.Business.AutoMapper;
using Rased.Business.Services.AuthServices;
using Rased.Business.Services.Goals;
using Rased.Business.Services.Savings;
using Rased.Infrastructure.Data;
using Rased.Infrastructure.Helpers.Constants;
using Rased.Infrastructure.Models.User;
using Rased.Infrastructure.Repositoryies.Base;
using Rased.Infrastructure.UnitsOfWork;
using System.Text;
using api5.Rased_API.Rased.Business.Services.Incomes;
using Microsoft.OpenApi.Models;
using Rased_API.Rased.Business.Services.BudgetService;
using Rased.Business.Services.ExpenseService;
using Rased.Business.Services.Wallets;
using Rased.Business.Services.Categories;
using Rased.Business.Services.SubCategories;
using Rased.Business.Services.Transfer;
using Rased.Business.Services.SharedWallets;
using Rased.Business.Services.Friendships;
using Rased.Business.Services.Bills;
using Rased.Infrastructure.Helpers;
using Rased.Business.Services.RecommendSystem;

namespace Rased.Api
{
    public class Program
    {
        public  static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // Add RasedDbContext Service To The Pipeline
            builder.Services.AddDbContext<RasedDbContext>(
                op => op.UseSqlServer(
                    builder.Configuration.GetConnectionString("cs"),
                    con => con.MigrationsAssembly(typeof(RasedDbContext).Assembly.FullName)
                )
            );


           

            //Identity
            builder.Services.AddIdentity<RasedUser, CustomRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                 options.User.RequireUniqueEmail = true;
             

            })
               .AddEntityFrameworkStores<RasedDbContext>()
               .AddDefaultTokenProviders();

            // Add JWT Authentication
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(Option =>
            {
                #region SecurityKey
                var SecretKeyString = builder.Configuration.GetSection("SecretKey").Value;
                var SecretKeyByte = Encoding.ASCII.GetBytes(SecretKeyString);
                SecurityKey securityKey = new SymmetricSecurityKey(SecretKeyByte);
                #endregion
                Option.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,

                    ValidateIssuer = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero  
                };
            });
            builder.Services.AddAuthorization();



            // Register Unit Of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Services Registeration
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISavingService, SavingService>();
            builder.Services.AddScoped<IGoalService, GoalService>();

            builder.Services.AddScoped<IWalletService, WalletService>();
            builder.Services.AddScoped<ISharedWalletService, SharedWalletService>();
            builder.Services.AddScoped<IFriendshipService, FriendshipService>();

            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
            builder.Services.AddScoped<IGoalTransactionService, GoalTransactionService>();

            builder.Services.AddScoped<ITransactionService,TransactionService >();
            builder.Services.AddScoped<IExpenseTransactionRecordService, ExpenseTransactionRecordService>();
            builder.Services.AddScoped<IPersonalIncomeTransactionRecordService, PersonalIncomeTransactionRecordService>();
            builder.Services.AddScoped<ISharedWalletIncomeTransactionService, SharedWalletIncomeTransactionService>();
            builder.Services.AddScoped<ITransactionApprovalService, TransactionApprovalService>();
            builder.Services.AddScoped<ITransactionRejectionService, TransactionRejectionService>();
            builder.Services.AddScoped<IStaticReceiverTypeDataService, StaticReceiverTypeDataService>();
            builder.Services.AddScoped<IStaticTransactionStatusService, StaticTransactionStatusService>();
            builder.Services.AddScoped<IBillService, BillService>();
            builder.Services.AddHttpClient();

            builder.Services.AddScoped<IRecommendationService, RecommendationService>();
            builder.Services.AddScoped<IAiRecommendationService, AiRecommendationService>();
            builder.Services.AddHostedService<MonthlyRecommendationBackgroundService>();



            builder.Services.AddScoped<IExpenseService, ExpenseService>();
            builder.Services.AddScoped<IExpenseTemplateService, ExpenseTemplateService>();
            builder.Services.AddScoped<IIncomeService, IncomeService>();
            builder.Services.AddScoped<IIncomeTemplateService, IncomeTemplateService>();
            builder.Services.AddScoped<IAttachmentService, AttachmentService>();
            builder.Services.AddScoped<IBudgetService, BudgetService>();
            builder.Services.AddScoped<IPaymentMethodsDataService, PaymentMethodsDataService>();
            builder.Services.AddScoped<IStaticIncomeSourceTypeDataService, StaticIncomeSourceTypeDataService>();

            //AutoMapping Registeration
            builder.Services.AddAutoMapper(map => map.AddProfile(new SavingProfile()));
            builder.Services.AddAutoMapper(map => map.AddProfile(new GoalProfile()));
            builder.Services.AddAutoMapper(map => map.AddProfile(new TransferProfile()));
            builder.Services.AddAutoMapper(map => map.AddProfile(new RecommProfile()));

            

            // Base Repository 
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));






            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            //builder.Services.AddOpenApi();

            //Swagger
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            builder.Services.AddSwaggerGen(c =>
            {
                // ����� JWT Bearer
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your token",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Rased - راصِــــــد", Version = "v1", Description = "AI-powered financial analytics application for comprehensive money management" });
                c.OperationFilter<SwaggerResponseTypesFilter>();
    
                // Enable XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // ? ��� ���� ������ ���� ����� Swagger ���� ��� IFormFile
                //c.OperationFilter<FileUploadOperationFilter>();
            });

            // Add CORS services
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });


            var app = builder.Build();

            // Enable CORS
            app.UseCors("AllowAll");

            // Call the SeedRoles method
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<CustomRole>>();
                await SeedRoles.SeedRole(roleManager);
            }


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapOpenApi();
            }

            app.UseHttpsRedirection();

          
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
