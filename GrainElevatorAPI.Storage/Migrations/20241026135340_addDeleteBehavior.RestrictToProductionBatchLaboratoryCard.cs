using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addDeleteBehaviorRestrictToProductionBatchLaboratoryCard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches",
                column: "LaboratoryCardId",
                principalTable: "LaboratoryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches",
                column: "LaboratoryCardId",
                principalTable: "LaboratoryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
