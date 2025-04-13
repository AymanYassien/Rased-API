using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangesMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.DropIndex(
                name: "IX_SharedWallets_StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "SharedWallets");

            migrationBuilder.DropColumn(
                name: "StaticColorTypeDataId",
                table: "SharedWallets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Wallets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Wallets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Wallets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SWInvitations",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "معلق",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldDefaultValue: "Pending");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "SharedWallets",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "SharedWallets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SharedWallets",
                type: "nvarchar(500)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_ColorTypeId",
                table: "SharedWallets",
                column: "ColorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_ColorTypeId",
                table: "SharedWallets",
                column: "ColorTypeId",
                principalTable: "StaticColorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_ColorTypeId",
                table: "SharedWallets");

            migrationBuilder.DropIndex(
                name: "IX_SharedWallets_ColorTypeId",
                table: "SharedWallets");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "Wallets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "Wallets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Wallets",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "SWInvitations",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "Pending",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldDefaultValue: "معلق");

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                table: "SharedWallets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Icon",
                table: "SharedWallets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "SharedWallets",
                type: "nvarchar(500)",
                maxLength: 200,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 200,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "SharedWallets",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StaticColorTypeDataId",
                table: "SharedWallets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SharedWallets_StaticColorTypeDataId",
                table: "SharedWallets",
                column: "StaticColorTypeDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedWallets_StaticColorTypes_StaticColorTypeDataId",
                table: "SharedWallets",
                column: "StaticColorTypeDataId",
                principalTable: "StaticColorTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
