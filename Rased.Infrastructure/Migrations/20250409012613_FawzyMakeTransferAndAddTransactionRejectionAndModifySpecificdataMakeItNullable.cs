using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FawzyMakeTransferAndAddTransactionRejectionAndModifySpecificdataMakeItNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AddColumn<string>(
                name: "IncomeSpecificData",
                table: "SharedWalletIncomeTransactions",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeSpecificData",
                table: "PersonalIncomeTrasactionRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseSpecificData",
                table: "ExpenseTransactionRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.CreateTable(
                name: "TransactionRejection",
                columns: table => new
                {
                    RejectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    RejectedById = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RejectedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRejection", x => x.RejectionId);
                    table.ForeignKey(
                        name: "FK_TransactionRejection_AspNetUsers_RejectedById",
                        column: x => x.RejectedById,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionRejection_Transactions_TransactionId",
                        column: x => x.TransactionId,
                        principalTable: "Transactions",
                        principalColumn: "TransactionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "StaticReceiverTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Friend" },
                    { 2, "SharedWallet" },
                    { 3, "UnknownPerson" }
                });

            migrationBuilder.InsertData(
                table: "StaticTransactionStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pending" },
                    { 2, "Approved" },
                    { 3, "Canceled" },
                    { 4, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRejection_RejectedById",
                table: "TransactionRejection",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRejection_TransactionId",
                table: "TransactionRejection",
                column: "TransactionId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionRejection_TransactionId_RejectedById",
                table: "TransactionRejection",
                columns: new[] { "TransactionId", "RejectedById" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionRejection");

            migrationBuilder.DeleteData(
                table: "StaticReceiverTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StaticReceiverTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StaticReceiverTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StaticTransactionStatus",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "StaticTransactionStatus",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "StaticTransactionStatus",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "StaticTransactionStatus",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "IncomeSpecificData",
                table: "SharedWalletIncomeTransactions");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Transactions",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "IncomeSpecificData",
                table: "PersonalIncomeTrasactionRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpenseSpecificData",
                table: "ExpenseTransactionRecords",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);
        }
    }
}
