using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class moveBaseValuesInRegister : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoistureBase",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "WeedImpurityBase",
                table: "ProductionBatches");

            migrationBuilder.AddColumn<double>(
                name: "MoistureBase",
                table: "Registers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeedImpurityBase",
                table: "Registers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MoistureBase",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "WeedImpurityBase",
                table: "Registers");

            migrationBuilder.AddColumn<double>(
                name: "MoistureBase",
                table: "ProductionBatches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "WeedImpurityBase",
                table: "ProductionBatches",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
