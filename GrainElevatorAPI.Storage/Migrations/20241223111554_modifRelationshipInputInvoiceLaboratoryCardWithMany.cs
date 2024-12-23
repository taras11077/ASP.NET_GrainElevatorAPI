using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class modifRelationshipInputInvoiceLaboratoryCardWithMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "LaboratoryCardId",
                table: "InputInvoices");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.AddColumn<int>(
                name: "LaboratoryCardId",
                table: "InputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                unique: true);
        }
    }
}
