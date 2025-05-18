using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RasedAuthUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "AspNetUsers",
                newName: "FullName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "AspNetUsers",
                type: "DATETIME2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "AspNetUsers",
                type: "NVARCHAR(20)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "BIT",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "AccountStatus",
                table: "AspNetUsers",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "غير نشط");

            migrationBuilder.AddColumn<DateTime>(
                name: "BanBrokeAt",
                table: "AspNetUsers",
                type: "DATETIME2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannedDuration",
                table: "AspNetUsers",
                type: "NVARCHAR(20)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BannedReason",
                table: "AspNetUsers",
                type: "NVARCHAR(MAX)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FailedAttempts",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserBadge",
                table: "AspNetUsers",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "جديد");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountStatus",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BanBrokeAt",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BannedDuration",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BannedReason",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FailedAttempts",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserBadge",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "FullName",
                table: "AspNetUsers",
                newName: "LastName");

            migrationBuilder.AlterColumn<DateTime>(
                name: "OtpExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "DATETIME2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OTP",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(20)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "BIT",
                oldDefaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "NVARCHAR(255)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefreshTokenExpiryTime",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true);
        }
    }
}
