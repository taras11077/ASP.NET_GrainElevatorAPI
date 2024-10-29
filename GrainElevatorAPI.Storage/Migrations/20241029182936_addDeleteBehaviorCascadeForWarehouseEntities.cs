using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addDeleteBehaviorCascadeForWarehouseEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories",
                column: "WarehouseUnitId",
                principalTable: "WarehouseUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories",
                column: "WarehouseUnitId",
                principalTable: "WarehouseUnits",
                principalColumn: "Id");
        }
    }
}
