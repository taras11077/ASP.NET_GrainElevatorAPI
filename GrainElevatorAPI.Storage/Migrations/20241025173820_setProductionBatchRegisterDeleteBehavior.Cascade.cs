using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class setProductionBatchRegisterDeleteBehaviorCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id");
        }
    }
}
