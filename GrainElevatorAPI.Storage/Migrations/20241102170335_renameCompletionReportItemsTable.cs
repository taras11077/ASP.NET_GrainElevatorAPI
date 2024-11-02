using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class renameCompletionReportItemsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceListItems_PriceLists_ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.DropTable(
                name: "CompletionReportItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceListItems_ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.RenameColumn(
                name: "OperationTitle",
                table: "PriceListItems",
                newName: "OperationName");

            migrationBuilder.RenameColumn(
                name: "QuantitiesDrying",
                table: "CompletionReports",
                newName: "ReportQuantitiesDrying");

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "PriceLists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "OperationPrice",
                table: "PriceListItems",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "TechnologicalOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                        column: x => x.CompletionReportId,
                        principalTable: "CompletionReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_ProductId",
                table: "PriceLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_CompletionReportId",
                table: "TechnologicalOperations",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_CreatedById",
                table: "TechnologicalOperations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_ModifiedById",
                table: "TechnologicalOperations",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_RemovedById",
                table: "TechnologicalOperations",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_RestoreById",
                table: "TechnologicalOperations",
                column: "RestoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceListItems_PriceLists_PriceListId",
                table: "PriceListItems",
                column: "PriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Products_ProductId",
                table: "PriceLists",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceListItems_PriceLists_PriceListId",
                table: "PriceListItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Products_ProductId",
                table: "PriceLists");

            migrationBuilder.DropTable(
                name: "TechnologicalOperations");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_ProductId",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "CompletionReports");

            migrationBuilder.RenameColumn(
                name: "OperationName",
                table: "PriceListItems",
                newName: "OperationTitle");

            migrationBuilder.RenameColumn(
                name: "ReportQuantitiesDrying",
                table: "CompletionReports",
                newName: "QuantitiesDrying");

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "PriceLists",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<double>(
                name: "OperationPrice",
                table: "PriceListItems",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ProductionPriceListId",
                table: "PriceListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CompletionReportItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TechnologicalOperation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionReportItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletionReportItems_CompletionReports_CompletionReportId",
                        column: x => x.CompletionReportId,
                        principalTable: "CompletionReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletionReportItems_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportItems_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportItems_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportItems_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_ProductionPriceListId",
                table: "PriceListItems",
                column: "ProductionPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportItems_CompletionReportId",
                table: "CompletionReportItems",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportItems_CreatedById",
                table: "CompletionReportItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportItems_ModifiedById",
                table: "CompletionReportItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportItems_RemovedById",
                table: "CompletionReportItems",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportItems_RestoreById",
                table: "CompletionReportItems",
                column: "RestoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceListItems_PriceLists_ProductionPriceListId",
                table: "PriceListItems",
                column: "ProductionPriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
