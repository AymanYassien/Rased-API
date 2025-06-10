using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIncomeSubCatRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incomes_SubCategories_SubCategoryId",
                table: "Incomes");

            migrationBuilder.DropIndex(
                name: "IX_Incomes_SubCategoryId",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Incomes");

            migrationBuilder.DropColumn(
                name: "SubCategoryId",
                table: "Incomes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Incomes",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubCategoryId",
                table: "Incomes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Incomes_SubCategoryId",
                table: "Incomes",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incomes_SubCategories_SubCategoryId",
                table: "Incomes",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "SubCategoryId");
        }
    }
}
