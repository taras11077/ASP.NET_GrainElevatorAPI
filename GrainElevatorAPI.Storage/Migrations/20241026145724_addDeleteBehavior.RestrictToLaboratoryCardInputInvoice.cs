using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addDeleteBehaviorRestrictToLaboratoryCardInputInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                principalTable: "InputInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                principalTable: "InputInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
