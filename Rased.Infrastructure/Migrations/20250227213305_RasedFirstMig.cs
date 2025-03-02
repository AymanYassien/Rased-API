using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RasedFirstMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    LastName = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "DATE", nullable: true),
                    ProfilePic = table.Column<byte[]>(type: "IMAGE", nullable: true),
                    Country = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Address = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true ),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CategoryTypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Symbol = table.Column<string>(type: "NVARCHAR(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Price = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false),
                    DurationInDays = table.Column<int>(type: "INT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticBudgetTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticBudgetTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticColorTypes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticColorTypes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "StaticDaysOfWeekNames",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticDaysOfWeekNames", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticFriendRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFriendRequestStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticFriendshipStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticFriendshipStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticIncomeSourceTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticIncomeSourceTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticPaymentMethods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticPaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticReceiverTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticReceiverTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticSharedWalletAccessLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticSharedWalletAccessLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticThresholdTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticThresholdTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticTransactionStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTransactionStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticTriggerTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticTriggerTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StaticWalletStatus",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaticWalletStatus", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TitleEn = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    TitleAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    MessageEn = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    MessageAr = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    url = table.Column<string>(type: "NVARCHAR(MAX)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Theme = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    DateFormat = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    MoneyFormat = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    TimeZone = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCategories",
                columns: table => new
                {
                    SubCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCategories", x => x.SubCategoryId);
                    table.ForeignKey(
                        name: "FK_SubCategories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Type = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanDetails_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Subscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(20)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subscriptions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Subscriptions_Plans_PlanId",
                        column: x => x.PlanId,
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FriendRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    FriendRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendRequests_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FriendRequests_StaticFriendRequestStatus_FriendRequestStatusId",
                        column: x => x.FriendRequestStatusId,
                        principalTable: "StaticFriendRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    FriendshipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId1 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId2 = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false),
                    Nickname1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Nickname2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FriendshipStatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => x.FriendshipId);
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId1",
                        column: x => x.UserId1,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_AspNetUsers_UserId2",
                        column: x => x.UserId2,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Friendships_StaticFriendshipStatus_FriendshipStatusId",
                        column: x => x.FriendshipStatusId,
                        principalTable: "StaticFriendshipStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutomationRules",
                columns: table => new
                {
                    AutomationRuleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true),
                    TriggerTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutomationRules", x => x.AutomationRuleId);
                    table.ForeignKey(
                        name: "FK_AutomationRules_StaticTriggerTypes_TriggerTypeId",
                        column: x => x.TriggerTypeId,
                        principalTable: "StaticTriggerTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedWallets",
                columns: table => new
                {
                    SharedWalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 200, nullable: false),
                    Color = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalletStatusId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedWallets", x => x.SharedWalletId);
                    table.ForeignKey(
                        name: "FK_SharedWallets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedWallets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedWallets_StaticWalletStatus_WalletStatusId",
                        column: x => x.WalletStatusId,
                        principalTable: "StaticWalletStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TotalBalance = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    ExpenseLimit = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WalletStatusId = table.Column<int>(type: "int", nullable: false),
                    ColorTypeId = table.Column<int>(type: "int", nullable: false),
                    CurrencyId = table.Column<int>(type: "int", nullable: false),
                    CreatorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_Wallets_AspNetUsers_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_Currencies_CurrencyId",
                        column: x => x.CurrencyId,
                        principalTable: "Currencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_StaticColorTypes_ColorTypeId",
                        column: x => x.ColorTypeId,
                        principalTable: "StaticColorTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Wallets_StaticWalletStatus_WalletStatusId",
                        column: x => x.WalletStatusId,
                        principalTable: "StaticWalletStatus",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    EnableEmails = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    EnableAll = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    UserPrefId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSettings_UserPreferences_UserPrefId",
                        column: x => x.UserPrefId,
                        principalTable: "UserPreferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedWalletMembers",
                columns: table => new
                {
                    MembershipId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SharedWalletId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AccessLevelId = table.Column<int>(type: "int", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedWalletMembers", x => x.MembershipId);
                    table.ForeignKey(
                        name: "FK_SharedWalletMembers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SharedWalletMembers_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_SharedWalletMembers_StaticSharedWalletAccessLevels_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "StaticSharedWalletAccessLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SenderWalletId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ReceiverWalletId = table.Column<int>(type: "int", nullable: true),
                    ReceiverTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TransactionStatusId = table.Column<int>(type: "int", nullable: false),
                    DisplayColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsReadOnly = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.CheckConstraint("CK_Transaction_Amount", "[Amount] > 0");
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_SharedWallets_ReceiverWalletId",
                        column: x => x.ReceiverWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Transactions_SharedWallets_SenderWalletId",
                        column: x => x.SenderWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Transactions_StaticReceiverTypes_ReceiverTypeId",
                        column: x => x.ReceiverTypeId,
                        principalTable: "StaticReceiverTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_StaticTransactionStatus_TransactionStatusId",
                        column: x => x.TransactionStatusId,
                        principalTable: "StaticTransactionStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    BudgetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    BudgetAmount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpentAmount = table.Column<decimal>(type: "decimal(8,2)", nullable: false, defaultValue: 0.00m),
                    RemainingAmount = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    RolloverUnspent = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    BudgetTypeId = table.Column<int>(type: "int", nullable: false),
                    DayOfMonth = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.BudgetId);
                    table.CheckConstraint("CK_Budget_DateRange_Valid", "[EndDate] > [StartDate]");
                    table.CheckConstraint("CK_Budget_SpentAmount_NonNegative", "[SpentAmount] >= 0");
                    table.CheckConstraint("CK_Budget_WalletAssignment", "([WalletId] IS NULL AND [SharedWalletId] IS NOT NULL) OR ([WalletId] IS NOT NULL AND [SharedWalletId] IS NULL)");
                    table.ForeignKey(
                        name: "FK_Budgets_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Budgets_StaticBudgetTypes_BudgetTypeId",
                        column: x => x.BudgetTypeId,
                        principalTable: "StaticBudgetTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Budgets_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Budgets_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTemplates",
                columns: table => new
                {
                    TemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    AutomationRuleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", precision: 8, scale: 2, nullable: false),
                    IsNeedApprovalWhenAutoAdd = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTemplates", x => x.TemplateId);
                    table.CheckConstraint("CK_Expense_WalletOrSharedWallet1", "(WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL)");
                    table.CheckConstraint("CK_ExpenseTemplate_Amount_Positive", "[Amount] >= 0");
                    table.ForeignKey(
                        name: "FK_ExpenseTemplates_AutomationRules_AutomationRuleId",
                        column: x => x.AutomationRuleId,
                        principalTable: "AutomationRules",
                        principalColumn: "AutomationRuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseTemplates_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_ExpenseTemplates_StaticPaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "StaticPaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseTemplates_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseTemplates_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "Goals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    CategoryName = table.Column<string>(type: "NVARCHAR(255)", nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    Status = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    StartedDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    DesiredDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    StartedAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    CurrentAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    TargetAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    IsTemplate = table.Column<bool>(type: "BIT", nullable: false, defaultValue: false),
                    Frequency = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    FrequencyAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    SubCatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Goals_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Goals_SubCategories_SubCatId",
                        column: x => x.SubCatId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_Goals_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "Incomes",
                columns: table => new
                {
                    IncomeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    IncomeSourceTypeId = table.Column<int>(type: "int", nullable: false),
                    IncomeTemplateId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsAutomated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incomes", x => x.IncomeId);
                    table.CheckConstraint("CK_Income_WalletOrSharedWallet", "((WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL))");
                    table.ForeignKey(
                        name: "FK_Incomes_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Incomes_StaticIncomeSourceTypes_IncomeSourceTypeId",
                        column: x => x.IncomeSourceTypeId,
                        principalTable: "StaticIncomeSourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incomes_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_Incomes_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "IncomeTemplates",
                columns: table => new
                {
                    IncomeTemplateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    AutomationRuleId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IncomeSourceTypeId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    IsNeedApprovalWhenAutoAdd = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeTemplates", x => x.IncomeTemplateId);
                    table.CheckConstraint("CK_IncomeTemplate_WalletOrSharedWallet", "((WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL))");
                    table.ForeignKey(
                        name: "FK_IncomeTemplates_AutomationRules_AutomationRuleId",
                        column: x => x.AutomationRuleId,
                        principalTable: "AutomationRules",
                        principalColumn: "AutomationRuleId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeTemplates_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_IncomeTemplates_StaticIncomeSourceTypes_IncomeSourceTypeId",
                        column: x => x.IncomeSourceTypeId,
                        principalTable: "StaticIncomeSourceTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncomeTemplates_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_IncomeTemplates_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    InstallmentAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    TotalInstallments = table.Column<int>(type: "INT", nullable: false),
                    PaidInstallments = table.Column<int>(type: "INT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loans_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Loans_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "Savings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "DECIMAL(9,6)", nullable: false),
                    IsSaving = table.Column<bool>(type: "BIT", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    SubCatId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Savings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Savings_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Savings_SubCategories_SubCatId",
                        column: x => x.SubCatId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_Savings_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "WalletStatistics",
                columns: table => new
                {
                    WalletId = table.Column<int>(type: "int", nullable: false),
                    TotalIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalSavings = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalLoans = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletStatistics", x => x.WalletId);
                    table.ForeignKey(
                        name: "FK_WalletStatistics_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "TransactionApprovals",
                columns: table => new
                {
                    ApprovalId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionApprovals", x => x.ApprovalId);
                    table.ForeignKey(
                        name: "FK_TransactionApprovals_AspNetUsers_ApproverId",
                        column: x => x.ApproverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionApprovals_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    SubCategoryId = table.Column<int>(type: "int", nullable: true),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    IsAutomated = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    RelatedBudgetId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.CheckConstraint("CK_Expense_Amount_GreaterThanZero", "Amount > 0");
                    table.CheckConstraint("CK_Expense_WalletOrSharedWallet", "(WalletId IS NULL AND SharedWalletId IS NOT NULL) OR (WalletId IS NOT NULL AND SharedWalletId IS NULL)");
                    table.ForeignKey(
                        name: "FK_Expenses_Budgets_RelatedBudgetId",
                        column: x => x.RelatedBudgetId,
                        principalTable: "Budgets",
                        principalColumn: "BudgetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_Expenses_StaticPaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "StaticPaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Expenses_SubCategories_SubCategoryId",
                        column: x => x.SubCategoryId,
                        principalTable: "SubCategories",
                        principalColumn: "SubCategoryId");
                    table.ForeignKey(
                        name: "FK_Expenses_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateTable(
                name: "GoalTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsertedAmount = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    InsertedDate = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    GoalId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GoalTransactions_Goals_GoalId",
                        column: x => x.GoalId,
                        principalTable: "Goals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoanInstallments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AmountToPay = table.Column<decimal>(type: "DECIMAL(12,9)", nullable: false),
                    DateToPay = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(50)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME2", nullable: true),
                    LoanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanInstallments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoanInstallments_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalIncomeTrasactionRecords",
                columns: table => new
                {
                    PersonalIncomeTrasactionRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    IncomeId = table.Column<int>(type: "int", nullable: false),
                    ApprovalId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IncomeSpecificData = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalIncomeTrasactionRecords", x => x.PersonalIncomeTrasactionRecordId);
                    table.ForeignKey(
                        name: "FK_PersonalIncomeTrasactionRecords_Incomes_IncomeId",
                        column: x => x.IncomeId,
                        principalTable: "Incomes",
                        principalColumn: "IncomeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PersonalIncomeTrasactionRecords_TransactionApprovals_ApprovalId",
                        column: x => x.ApprovalId,
                        principalTable: "TransactionApprovals",
                        principalColumn: "ApprovalId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalIncomeTrasactionRecords_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SharedWalletIncomeTransactions",
                columns: table => new
                {
                    SharedWalletIncomeTransactionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    IncomeId = table.Column<int>(type: "int", nullable: false),
                    ApprovalId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SharedWalletIncomeTransactions", x => x.SharedWalletIncomeTransactionId);
                    table.ForeignKey(
                        name: "FK_SharedWalletIncomeTransactions_Incomes_IncomeId",
                        column: x => x.IncomeId,
                        principalTable: "Incomes",
                        principalColumn: "IncomeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedWalletIncomeTransactions_TransactionApprovals_ApprovalId",
                        column: x => x.ApprovalId,
                        principalTable: "TransactionApprovals",
                        principalColumn: "ApprovalId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SharedWalletIncomeTransactions_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId");
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    AttachmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    UploadDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    FileSize = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.AttachmentId);
                    table.ForeignKey(
                        name: "FK_Attachments_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseTransactionRecords",
                columns: table => new
                {
                    ExpenseTrasactionRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpenseSpecificData = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseTransactionRecords", x => x.ExpenseTrasactionRecordId);
                    table.ForeignKey(
                        name: "FK_ExpenseTransactionRecords_Expenses_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "Expenses",
                        principalColumn: "ExpenseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseTransactionRecords_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Attachment_ExpenseId",
                table: "Attachments",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationRules_TriggerTypeId",
                table: "AutomationRules",
                column: "TriggerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_BudgetTypeId",
                table: "Budgets",
                column: "BudgetTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_SharedWalletId",
                table: "Budgets",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_SubCategoryId",
                table: "Budgets",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_WalletId",
                table: "Budgets",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_PaymentMethodId",
                table: "Expenses",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_RelatedBudgetId",
                table: "Expenses",
                column: "RelatedBudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SharedWalletId",
                table: "Expenses",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SubCategoryId",
                table: "Expenses",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_WalletId",
                table: "Expenses",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplate_AutomationRuleId",
                table: "ExpenseTemplates",
                column: "AutomationRuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplate_Name",
                table: "ExpenseTemplates",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplates_PaymentMethodId",
                table: "ExpenseTemplates",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplates_SharedWalletId",
                table: "ExpenseTemplates",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplates_SubCategoryId",
                table: "ExpenseTemplates",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTemplates_WalletId",
                table: "ExpenseTemplates",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransactionRecord_ExpenseId",
                table: "ExpenseTransactionRecords",
                column: "ExpenseId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransactionRecord_TransactionId",
                table: "ExpenseTransactionRecords",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTransactionRecord_TransactionId_ExpenseId",
                table: "ExpenseTransactionRecords",
                columns: new[] { "TransactionId", "ExpenseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_ReceiverId",
                table: "FriendRequests",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequest_SenderId",
                table: "FriendRequests",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_FriendRequests_FriendRequestStatusId",
                table: "FriendRequests",
                column: "FriendRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserId1",
                table: "Friendships",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserId1_UserId2",
                table: "Friendships",
                columns: new[] { "UserId1", "UserId2" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Friendship_UserId2",
                table: "Friendships",
                column: "UserId2");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_FriendshipStatusId",
                table: "Friendships",
                column: "FriendshipStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_SharedWalletId",
                table: "Goals",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_SubCatId",
                table: "Goals",
                column: "SubCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Goals_WalletId",
                table: "Goals",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_GoalTransactions_GoalId",
                table: "GoalTransactions",
                column: "GoalId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_IncomeSourceTypeId",
                table: "Incomes",
                column: "IncomeSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_SharedWalletId",
                table: "Incomes",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_SubCategoryId",
                table: "Incomes",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_WalletId",
                table: "Incomes",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTemplates_AutomationRuleId",
                table: "IncomeTemplates",
                column: "AutomationRuleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTemplates_IncomeSourceTypeId",
                table: "IncomeTemplates",
                column: "IncomeSourceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTemplates_SharedWalletId",
                table: "IncomeTemplates",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTemplates_SubCategoryId",
                table: "IncomeTemplates",
                column: "SubCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomeTemplates_WalletId",
                table: "IncomeTemplates",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanInstallments_LoanId",
                table: "LoanInstallments",
                column: "LoanId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_SharedWalletId",
                table: "Loans",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Loans_WalletId",
                table: "Loans",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserPrefId",
                table: "NotificationSettings",
                column: "UserPrefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PersonalIncomeTransactionRecord_ApprovalId",
                table: "PersonalIncomeTrasactionRecords",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalIncomeTransactionRecord_IncomeId",
                table: "PersonalIncomeTrasactionRecords",
                column: "IncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalIncomeTransactionRecord_TransactionId",
                table: "PersonalIncomeTrasactionRecords",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalIncomeTransactionRecord_TransactionId_IncomeId_ApprovalId",
                table: "PersonalIncomeTrasactionRecords",
                columns: new[] { "TransactionId", "IncomeId", "ApprovalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanDetails_PlanId",
                table: "PlanDetails",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_SharedWalletId",
                table: "Savings",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_SubCatId",
                table: "Savings",
                column: "SubCatId");

            migrationBuilder.CreateIndex(
                name: "IX_Savings_WalletId",
                table: "Savings",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletIncomeTransaction_ApprovalId",
                table: "SharedWalletIncomeTransactions",
                column: "ApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletIncomeTransaction_IncomeId",
                table: "SharedWalletIncomeTransactions",
                column: "IncomeId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletIncomeTransaction_TransactionId",
                table: "SharedWalletIncomeTransactions",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletIncomeTransaction_TransactionId_IncomeId_ApprovalId",
                table: "SharedWalletIncomeTransactions",
                columns: new[] { "TransactionId", "IncomeId", "ApprovalId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletMembers_SharedWalletId",
                table: "SharedWalletMembers",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletMembers_UserId",
                table: "SharedWalletMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWalletMembers_UserId_SharedWalletId",
                table: "SharedWalletMembers",
                columns: new[] { "UserId", "SharedWalletId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_CreatorId",
                table: "SharedWallets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_CurrencyId",
                table: "SharedWallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_WalletStatusId",
                table: "SharedWallets",
                column: "WalletStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCategories_ParentCategoryId",
                table: "SubCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_PlanId",
                table: "Subscriptions",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionApproval_ApproverId",
                table: "TransactionApprovals",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionApproval_TransactionId",
                table: "TransactionApprovals",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionApproval_TransactionId_ApproverId",
                table: "TransactionApprovals",
                columns: new[] { "TransactionId", "ApproverId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ReceiverId",
                table: "Transactions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SenderId",
                table: "Transactions",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_Status",
                table: "Transactions",
                column: "TransactionStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverTypeId",
                table: "Transactions",
                column: "ReceiverTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreferences_UserId",
                table: "UserPreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_ColorTypeId",
                table: "Wallets",
                column: "ColorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CreatorId",
                table: "Wallets",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_CurrencyId",
                table: "Wallets",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_WalletStatusId",
                table: "Wallets",
                column: "WalletStatusId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Attachments");

            migrationBuilder.DropTable(
                name: "ExpenseTemplates");

            migrationBuilder.DropTable(
                name: "ExpenseTransactionRecords");

            migrationBuilder.DropTable(
                name: "FriendRequests");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "GoalTransactions");

            migrationBuilder.DropTable(
                name: "IncomeTemplates");

            migrationBuilder.DropTable(
                name: "LoanInstallments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "NotificationSettings");

            migrationBuilder.DropTable(
                name: "PersonalIncomeTrasactionRecords");

            migrationBuilder.DropTable(
                name: "PlanDetails");

            migrationBuilder.DropTable(
                name: "Savings");

            migrationBuilder.DropTable(
                name: "SharedWalletIncomeTransactions");

            migrationBuilder.DropTable(
                name: "SharedWalletMembers");

            migrationBuilder.DropTable(
                name: "StaticDaysOfWeekNames");

            migrationBuilder.DropTable(
                name: "StaticThresholdTypes");

            migrationBuilder.DropTable(
                name: "Subscriptions");

            migrationBuilder.DropTable(
                name: "WalletStatistics");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "StaticFriendRequestStatus");

            migrationBuilder.DropTable(
                name: "StaticFriendshipStatus");

            migrationBuilder.DropTable(
                name: "Goals");

            migrationBuilder.DropTable(
                name: "AutomationRules");

            migrationBuilder.DropTable(
                name: "Loans");

            migrationBuilder.DropTable(
                name: "UserPreferences");

            migrationBuilder.DropTable(
                name: "Incomes");

            migrationBuilder.DropTable(
                name: "TransactionApprovals");

            migrationBuilder.DropTable(
                name: "StaticSharedWalletAccessLevels");

            migrationBuilder.DropTable(
                name: "Plans");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "StaticPaymentMethods");

            migrationBuilder.DropTable(
                name: "StaticTriggerTypes");

            migrationBuilder.DropTable(
                name: "StaticIncomeSourceTypes");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "StaticBudgetTypes");

            migrationBuilder.DropTable(
                name: "SubCategories");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "SharedWallets");

            migrationBuilder.DropTable(
                name: "StaticReceiverTypes");

            migrationBuilder.DropTable(
                name: "StaticTransactionStatus");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "StaticColorTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "StaticWalletStatus");
        }
    }
}
