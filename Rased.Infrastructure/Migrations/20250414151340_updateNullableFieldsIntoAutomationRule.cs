using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Rased.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateNullableFieldsIntoAutomationRule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomationRules_StaticTriggerTypes_TriggerTypeId",
                table: "AutomationRules");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerTypeId",
                table: "AutomationRules",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomationRules_StaticTriggerTypes_TriggerTypeId",
                table: "AutomationRules",
                column: "TriggerTypeId",
                principalTable: "StaticTriggerTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomationRules_StaticTriggerTypes_TriggerTypeId",
                table: "AutomationRules");

            migrationBuilder.AlterColumn<int>(
                name: "TriggerTypeId",
                table: "AutomationRules",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AutomationRules_StaticTriggerTypes_TriggerTypeId",
                table: "AutomationRules",
                column: "TriggerTypeId",
                principalTable: "StaticTriggerTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
