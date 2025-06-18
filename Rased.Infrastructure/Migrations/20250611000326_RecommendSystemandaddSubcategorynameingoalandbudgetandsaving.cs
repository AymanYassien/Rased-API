using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecommendSystemandaddSubcategorynameingoalandbudgetandsaving : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetRecommendation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    WalletGroupId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsRead = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetRecommendation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetRecommendation_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BudgetRecommendation_SharedWallets_WalletGroupId",
                        column: x => x.WalletGroupId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_BudgetRecommendation_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "StaticReceiverTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 4, "PersonalWalletTransfer" });

            migrationBuilder.CreateIndex(
                name: "IX_BudgetRecommendation_UserId",
                table: "BudgetRecommendation",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetRecommendation_WalletGroupId",
                table: "BudgetRecommendation",
                column: "WalletGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetRecommendation_WalletId",
                table: "BudgetRecommendation",
                column: "WalletId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetRecommendation");

            migrationBuilder.DeleteData(
                table: "StaticReceiverTypes",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
