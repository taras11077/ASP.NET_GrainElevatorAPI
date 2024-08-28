using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class renameProductTitleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_ProductTitles_ProductTitleId",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_ProductTitles_ProductTitleId",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_ProductTitles_ProductTitleId",
                table: "InputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_ProductTitles_ProductTitleId",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_ProductTitles_ProductTitleId",
                table: "Registers");

            migrationBuilder.DropTable(
                name: "ProductTitles");

            migrationBuilder.DropIndex(
                name: "IX_Registers_ProductTitleId",
                table: "Registers");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_ProductTitleId",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_ProductTitleId",
                table: "InputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_ProductTitleId",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_CompletionReports_ProductTitleId",
                table: "CompletionReports");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "Registers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "OutputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "InputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "DepotItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "CompletionReports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ProductId",
                table: "Registers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ProductId",
                table: "OutputInvoices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ProductId",
                table: "InputInvoices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_ProductId",
                table: "DepotItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ProductId",
                table: "CompletionReports",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Products_ProductId",
                table: "CompletionReports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Products_ProductId",
                table: "DepotItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Products_ProductId",
                table: "InputInvoices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Products_ProductId",
                table: "OutputInvoices",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Products_ProductId",
                table: "Registers",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Products_ProductId",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Products_ProductId",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Products_ProductId",
                table: "InputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Products_ProductId",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Products_ProductId",
                table: "Registers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Registers_ProductId",
                table: "Registers");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_ProductId",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_ProductId",
                table: "InputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_ProductId",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_CompletionReports_ProductId",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "CompletionReports");

            migrationBuilder.CreateTable(
                name: "ProductTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTitles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ProductTitleId",
                table: "Registers",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ProductTitleId",
                table: "OutputInvoices",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ProductTitleId",
                table: "InputInvoices",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_ProductTitleId",
                table: "DepotItems",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ProductTitleId",
                table: "CompletionReports",
                column: "ProductTitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_ProductTitles_ProductTitleId",
                table: "CompletionReports",
                column: "ProductTitleId",
                principalTable: "ProductTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_ProductTitles_ProductTitleId",
                table: "DepotItems",
                column: "ProductTitleId",
                principalTable: "ProductTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_ProductTitles_ProductTitleId",
                table: "InputInvoices",
                column: "ProductTitleId",
                principalTable: "ProductTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_ProductTitles_ProductTitleId",
                table: "OutputInvoices",
                column: "ProductTitleId",
                principalTable: "ProductTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_ProductTitles_ProductTitleId",
                table: "Registers",
                column: "ProductTitleId",
                principalTable: "ProductTitles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
