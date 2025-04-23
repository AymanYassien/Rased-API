using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBillDraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillDraftId",
                table: "Attachments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "BillDrafts",
                columns: table => new
                {
                    BillDraftId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WalletId = table.Column<int>(type: "int", nullable: true),
                    SharedWalletId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "DECIMAL(18,2)", nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(255)", nullable: false),
                    Date = table.Column<DateTime>(type: "DATETIME2", nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(MAX)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillDrafts", x => x.BillDraftId);
                    table.ForeignKey(
                        name: "FK_BillDrafts_SharedWallets_SharedWalletId",
                        column: x => x.SharedWalletId,
                        principalTable: "SharedWallets",
                        principalColumn: "SharedWalletId");
                    table.ForeignKey(
                        name: "FK_BillDrafts_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "WalletId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_BillDraftId",
                table: "Attachments",
                column: "BillDraftId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDrafts_SharedWalletId",
                table: "BillDrafts",
                column: "SharedWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_BillDrafts_WalletId",
                table: "BillDrafts",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_BillDrafts_BillDraftId",
                table: "Attachments",
                column: "BillDraftId",
                principalTable: "BillDrafts",
                principalColumn: "BillDraftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_BillDrafts_BillDraftId",
                table: "Attachments");

            migrationBuilder.DropTable(
                name: "BillDrafts");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_BillDraftId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "BillDraftId",
                table: "Attachments");
        }
    }
}
