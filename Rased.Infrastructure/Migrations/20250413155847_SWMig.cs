using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SWMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryTypeId",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StaticWalletStatus",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "StaticColorTypes",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "SubCategories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ColorTypeId",
                table: "SharedWallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpenseLimit",
                table: "SharedWallets",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InitialBalance",
                table: "SharedWallets",
                type: "decimal(8,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "StaticColorTypeDataId",
                table: "SharedWallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StaticWalletStatusDataId",
                table: "SharedWallets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SWInvitations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SharedWalletId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ReceiverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<string>(type: "NVARCHAR(50)", nullable: false, defaultValue: "Pending"),
                    InvitedAt = table.Column<DateTime>(type: "DATETIME2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SWInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SWInvitations_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SWInvitations_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SWInvitations_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_StaticColorTypeDataId",
                table: "SharedWallets",
                column: "StaticColorTypeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_StaticWalletStatusDataId",
                table: "SharedWallets",
                column: "StaticWalletStatusDataId");

            migrationBuilder.CreateIndex(
                name: "IX_SWInvitations_ReceiverId",
                table: "SWInvitations",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_SWInvitations_SenderId",
                table: "SWInvitations",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_SWInvitations_SharedWalletId",
                table: "SWInvitations",
                column: "SharedWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_StaticColorTypeDataId",
                table: "SharedWallets",
                column: "StaticColorTypeDataId",
                principalTable: "StaticColorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SharedWallets_StaticWalletStatus_StaticWalletStatusDataId",
                table: "SharedWallets",
                column: "StaticWalletStatusDataId",
                principalTable: "StaticWalletStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.DropForeignKey(
                name: "FK_SharedWallets_StaticWalletStatus_StaticWalletStatusDataId",
                table: "SharedWallets");

            migrationBuilder.DropTable(
                name: "SWInvitations");

            migrationBuilder.DropIndex(
                name: "IX_SharedWallets_StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.DropIndex(
                name: "IX_SharedWallets_StaticWalletStatusDataId",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "SubCategories");

            migrationBuilder.DropColumn(
                name: "ColorTypeId",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "ExpenseLimit",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "InitialBalance",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "StaticWalletStatusDataId",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Categories");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StaticWalletStatus",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StaticColorTypes",
                newName: "id");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
