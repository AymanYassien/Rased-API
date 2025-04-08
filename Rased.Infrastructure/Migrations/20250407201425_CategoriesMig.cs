using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CategoriesMig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryTypeId",
                table: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "SubCategories",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "SubCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
