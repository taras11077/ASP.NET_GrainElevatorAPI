using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addPropertyIsFinalized : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "Registers",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "PriceLists",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "LaboratoryCards",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinalized",
                table: "InputInvoices",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "IsFinalized",
                table: "InputInvoices");
        }
    }
}
