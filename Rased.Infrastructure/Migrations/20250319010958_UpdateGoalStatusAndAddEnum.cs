using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGoalStatusAndAddEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Goals",
                type: "NVARCHAR(50)",
                nullable: false,
                defaultValue: "InProgress",
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goals",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(MAX)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Goals",
                type: "NVARCHAR(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(50)",
                oldDefaultValue: "InProgress");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Goals",
                type: "NVARCHAR(MAX)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
