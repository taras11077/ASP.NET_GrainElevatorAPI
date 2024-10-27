using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditableEntityFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_DepotItems_DepotItemId",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_DepotItems_DepotItemId",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceListItems_PriceLists_PriceListId",
                table: "PriceListItems");

            migrationBuilder.DropTable(
                name: "AppDefects");

            migrationBuilder.DropIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems");

            migrationBuilder.RenameColumn(
                name: "DepotItemId",
                table: "OutputInvoices",
                newName: "WarehouseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_OutputInvoices_DepotItemId",
                table: "OutputInvoices",
                newName: "IX_OutputInvoices_WarehouseUnitId");

            migrationBuilder.RenameColumn(
                name: "DepotItemId",
                table: "DepotProductCategories",
                newName: "WarehouseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_DepotItemId",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_WarehouseUnitId");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Suppliers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Registers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "ProductionBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceListItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ProductionPriceListId",
                table: "PriceListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "OutputInvoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "LaboratoryCards",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "InputInvoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "DepotProductCategories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "DepotItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "CompletionReports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "CompletionReportItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_ProductionPriceListId",
                table: "PriceListItems",
                column: "ProductionPriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ModifiedById",
                table: "Employees",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RemovedById",
                table: "Employees",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RestoreById",
                table: "Employees",
                column: "RestoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_DepotItems_WarehouseUnitId",
                table: "DepotProductCategories",
                column: "WarehouseUnitId",
                principalTable: "DepotItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_ModifiedById",
                table: "Employees",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_RemovedById",
                table: "Employees",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Employees_RestoreById",
                table: "Employees",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_DepotItems_WarehouseUnitId",
                table: "OutputInvoices",
                column: "WarehouseUnitId",
                principalTable: "DepotItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceListItems_PriceLists_ProductionPriceListId",
                table: "PriceListItems",
                column: "ProductionPriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_DepotItems_WarehouseUnitId",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_ModifiedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_RemovedById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Employees_RestoreById",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_DepotItems_WarehouseUnitId",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceListItems_PriceLists_ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.DropIndex(
                name: "IX_PriceListItems_ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.DropIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ModifiedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RemovedById",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RestoreById",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ProductionPriceListId",
                table: "PriceListItems");

            migrationBuilder.RenameColumn(
                name: "WarehouseUnitId",
                table: "OutputInvoices",
                newName: "DepotItemId");

            migrationBuilder.RenameIndex(
                name: "IX_OutputInvoices_WarehouseUnitId",
                table: "OutputInvoices",
                newName: "IX_OutputInvoices_DepotItemId");

            migrationBuilder.RenameColumn(
                name: "WarehouseUnitId",
                table: "DepotProductCategories",
                newName: "DepotItemId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_WarehouseUnitId",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_DepotItemId");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Suppliers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Registers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "ProductionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceListItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "OutputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "LaboratoryCards",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "InputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "DepotProductCategories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "DepotItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "CompletionReports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "CompletionReportItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AppDefects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDefects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDefects_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_AppDefects_CreatedById",
                table: "AppDefects",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_DepotItems_DepotItemId",
                table: "DepotProductCategories",
                column: "DepotItemId",
                principalTable: "DepotItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_DepotItems_DepotItemId",
                table: "OutputInvoices",
                column: "DepotItemId",
                principalTable: "DepotItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceListItems_PriceLists_PriceListId",
                table: "PriceListItems",
                column: "PriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
