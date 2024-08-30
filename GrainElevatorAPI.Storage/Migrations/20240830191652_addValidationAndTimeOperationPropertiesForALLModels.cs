using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class addValidationAndTimeOperationPropertiesForALLModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Employees_CreatedById",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_LaboratoryCards_LaboratoryCardId",
                table: "InputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_Employees_CreatedById",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Employees_CreatedById",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Employees_CreatedById",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatch_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatch_Registers_RegisterId",
                table: "ProductionBatch");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Employees_CreatedById",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                table: "TechnologicalOperations");

            migrationBuilder.DropTable(
                name: "DepotItemCategories");

            migrationBuilder.DropTable(
                name: "PricesByOperation");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_LaboratoryCardId",
                table: "InputInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionBatch",
                table: "ProductionBatch");

            migrationBuilder.DropColumn(
                name: "QuantityesDryingReg",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "Product",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "OutInvNumber",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "Removed",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "PhysicalWeightReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "QuantityesDrying",
                table: "CompletionReports");

            migrationBuilder.RenameTable(
                name: "ProductionBatch",
                newName: "ProductionBatches");

            migrationBuilder.RenameColumn(
                name: "CreatedByInt",
                table: "PriceLists",
                newName: "RestoreById");

            migrationBuilder.RenameColumn(
                name: "Weediness",
                table: "LaboratoryCards",
                newName: "WeedImpurity");

            migrationBuilder.RenameColumn(
                name: "WeedinessBase",
                table: "ProductionBatches",
                newName: "WeedImpurityBase");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionBatch_RegisterId",
                table: "ProductionBatches",
                newName: "IX_ProductionBatches_RegisterId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionBatch_LaboratoryCardId",
                table: "ProductionBatches",
                newName: "IX_ProductionBatches_LaboratoryCardId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TechnologicalOperations",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CompletionReportId",
                table: "TechnologicalOperations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "TechnologicalOperations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "TechnologicalOperations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "TechnologicalOperations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "TechnologicalOperations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "TechnologicalOperations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "TechnologicalOperations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "TechnologicalOperations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "TechnologicalOperations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Roles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "WasteReg",
                table: "Registers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ShrinkageReg",
                table: "Registers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "RegisterNumber",
                table: "Registers",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PhysicalWeightReg",
                table: "Registers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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
                name: "AccWeightReg",
                table: "Registers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Registers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "Registers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "Registers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantitiesDryingReg",
                table: "Registers",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "Registers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "Registers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "Registers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "Registers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "PriceLists",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductTitle",
                table: "PriceLists",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "PriceLists",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VehicleNumber",
                table: "OutputInvoices",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategory",
                table: "OutputInvoices",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "OutputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "OutputInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "InvoiceNumber",
                table: "OutputInvoices",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "OutputInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "OutputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "OutputInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "OutputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "OutputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "OutputInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SpecialNotes",
                table: "LaboratoryCards",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LabCardNumber",
                table: "LaboratoryCards",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LaboratoryCards",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "InputInvoiceId",
                table: "LaboratoryCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "LaboratoryCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "LaboratoryCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductionBatchId",
                table: "LaboratoryCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "LaboratoryCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "LaboratoryCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "LaboratoryCards",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "LaboratoryCards",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "DepotItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "DepotItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "DepotItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "DepotItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "DepotItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "DepotItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "DepotItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "DepotItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFinalized",
                table: "CompletionReports",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CompletionReports",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "CompletionReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantitiesDrying",
                table: "CompletionReports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "CompletionReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReportPhysicalWeight",
                table: "CompletionReports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "CompletionReports",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Waste",
                table: "ProductionBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "Shrinkage",
                table: "ProductionBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "RegisterId",
                table: "ProductionBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccountWeight",
                table: "ProductionBatches",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ProductionBatches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "ProductionBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "ProductionBatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "ProductionBatches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "ProductionBatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "ProductionBatches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "ProductionBatches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "ProductionBatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionBatches",
                table: "ProductionBatches",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepotProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Value = table.Column<int>(type: "int", nullable: true),
                    DepotItemId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotProductCategories_DepotItems_DepotItemId",
                        column: x => x.DepotItemId,
                        principalTable: "DepotItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DepotProductCategories_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepotProductCategories_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepotProductCategories_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepotProductCategories_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PriceListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OperationPrice = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true),
                    PriceListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ModifiedById",
                table: "Registers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_RemovedById",
                table: "Registers",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_RestoreById",
                table: "Registers",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_ModifiedById",
                table: "PriceLists",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_RemovedById",
                table: "PriceLists",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_RestoreById",
                table: "PriceLists",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ModifiedById",
                table: "OutputInvoices",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_RemovedById",
                table: "OutputInvoices",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_RestoreById",
                table: "OutputInvoices",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_ModifiedById",
                table: "LaboratoryCards",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_RemovedById",
                table: "LaboratoryCards",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_RestoreById",
                table: "LaboratoryCards",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_CreatedById",
                table: "DepotItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_ModifiedById",
                table: "DepotItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_RemovedById",
                table: "DepotItems",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_RestoreById",
                table: "DepotItems",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ModifiedById",
                table: "CompletionReports",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_RemovedById",
                table: "CompletionReports",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_RestoreById",
                table: "CompletionReports",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_CreatedById",
                table: "ProductionBatches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_ModifiedById",
                table: "ProductionBatches",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_RemovedById",
                table: "ProductionBatches",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_RestoreById",
                table: "ProductionBatches",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotProductCategories_CreatedById",
                table: "DepotProductCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotProductCategories_DepotItemId",
                table: "DepotProductCategories",
                column: "DepotItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotProductCategories_ModifiedById",
                table: "DepotProductCategories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotProductCategories_RemovedById",
                table: "DepotProductCategories",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_DepotProductCategories_RestoreById",
                table: "DepotProductCategories",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_CreatedById",
                table: "PriceListItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_ModifiedById",
                table: "PriceListItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_RemovedById",
                table: "PriceListItems",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_RestoreById",
                table: "PriceListItems",
                column: "RestoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_CreatedById",
                table: "CompletionReports",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_ModifiedById",
                table: "CompletionReports",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_RemovedById",
                table: "CompletionReports",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_RestoreById",
                table: "CompletionReports",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Employees_CreatedById",
                table: "DepotItems",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Employees_ModifiedById",
                table: "DepotItems",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Employees_RemovedById",
                table: "DepotItems",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Employees_RestoreById",
                table: "DepotItems",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_Employees_CreatedById",
                table: "LaboratoryCards",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_Employees_ModifiedById",
                table: "LaboratoryCards",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_Employees_RemovedById",
                table: "LaboratoryCards",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_Employees_RestoreById",
                table: "LaboratoryCards",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                principalTable: "InputInvoices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Employees_CreatedById",
                table: "OutputInvoices",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Employees_ModifiedById",
                table: "OutputInvoices",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Employees_RemovedById",
                table: "OutputInvoices",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Employees_RestoreById",
                table: "OutputInvoices",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Employees_CreatedById",
                table: "PriceLists",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Employees_ModifiedById",
                table: "PriceLists",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Employees_RemovedById",
                table: "PriceLists",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Employees_RestoreById",
                table: "PriceLists",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Employees_CreatedById",
                table: "ProductionBatches",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Employees_ModifiedById",
                table: "ProductionBatches",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Employees_RemovedById",
                table: "ProductionBatches",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Employees_RestoreById",
                table: "ProductionBatches",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches",
                column: "LaboratoryCardId",
                principalTable: "LaboratoryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Employees_CreatedById",
                table: "Registers",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Employees_ModifiedById",
                table: "Registers",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Employees_RemovedById",
                table: "Registers",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Employees_RestoreById",
                table: "Registers",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                table: "TechnologicalOperations",
                column: "CompletionReportId",
                principalTable: "CompletionReports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_CreatedById",
                table: "TechnologicalOperations",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_ModifiedById",
                table: "TechnologicalOperations",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_RemovedById",
                table: "TechnologicalOperations",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_RestoreById",
                table: "TechnologicalOperations",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Employees_CreatedById",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Employees_ModifiedById",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Employees_RemovedById",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReports_Employees_RestoreById",
                table: "CompletionReports");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Employees_CreatedById",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Employees_ModifiedById",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Employees_RemovedById",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Employees_RestoreById",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_Employees_CreatedById",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_Employees_ModifiedById",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_Employees_RemovedById",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_Employees_RestoreById",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Employees_CreatedById",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Employees_ModifiedById",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Employees_RemovedById",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_Employees_RestoreById",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Employees_CreatedById",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Employees_ModifiedById",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Employees_RemovedById",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_PriceLists_Employees_RestoreById",
                table: "PriceLists");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Employees_CreatedById",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Employees_ModifiedById",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Employees_RemovedById",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Employees_RestoreById",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductionBatches_Registers_RegisterId",
                table: "ProductionBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Employees_CreatedById",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Employees_ModifiedById",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Employees_RemovedById",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_Registers_Employees_RestoreById",
                table: "Registers");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                table: "TechnologicalOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_Employees_CreatedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_Employees_ModifiedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_Employees_RemovedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropForeignKey(
                name: "FK_TechnologicalOperations_Employees_RestoreById",
                table: "TechnologicalOperations");

            migrationBuilder.DropTable(
                name: "DepotProductCategories");

            migrationBuilder.DropTable(
                name: "PriceListItems");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalOperations_CreatedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalOperations_ModifiedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalOperations_RemovedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalOperations_RestoreById",
                table: "TechnologicalOperations");

            migrationBuilder.DropIndex(
                name: "IX_Registers_ModifiedById",
                table: "Registers");

            migrationBuilder.DropIndex(
                name: "IX_Registers_RemovedById",
                table: "Registers");

            migrationBuilder.DropIndex(
                name: "IX_Registers_RestoreById",
                table: "Registers");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_ModifiedById",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_RemovedById",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_PriceLists_RestoreById",
                table: "PriceLists");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_ModifiedById",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_RemovedById",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_OutputInvoices_RestoreById",
                table: "OutputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_ModifiedById",
                table: "LaboratoryCards");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_RemovedById",
                table: "LaboratoryCards");

            migrationBuilder.DropIndex(
                name: "IX_LaboratoryCards_RestoreById",
                table: "LaboratoryCards");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_CreatedById",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_ModifiedById",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_RemovedById",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_DepotItems_RestoreById",
                table: "DepotItems");

            migrationBuilder.DropIndex(
                name: "IX_CompletionReports_ModifiedById",
                table: "CompletionReports");

            migrationBuilder.DropIndex(
                name: "IX_CompletionReports_RemovedById",
                table: "CompletionReports");

            migrationBuilder.DropIndex(
                name: "IX_CompletionReports_RestoreById",
                table: "CompletionReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductionBatches",
                table: "ProductionBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProductionBatches_CreatedById",
                table: "ProductionBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProductionBatches_ModifiedById",
                table: "ProductionBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProductionBatches_RemovedById",
                table: "ProductionBatches");

            migrationBuilder.DropIndex(
                name: "IX_ProductionBatches_RestoreById",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "QuantitiesDryingReg",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "Registers");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "ProductTitle",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "PriceLists");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "InvoiceNumber",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "OutputInvoices");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "InputInvoiceId",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "ProductionBatchId",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "LaboratoryCards");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "DepotItems");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "QuantitiesDrying",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ReportPhysicalWeight",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "ProductionBatches");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "ProductionBatches");

            migrationBuilder.RenameTable(
                name: "ProductionBatches",
                newName: "ProductionBatch");

            migrationBuilder.RenameColumn(
                name: "RestoreById",
                table: "PriceLists",
                newName: "CreatedByInt");

            migrationBuilder.RenameColumn(
                name: "WeedImpurity",
                table: "LaboratoryCards",
                newName: "Weediness");

            migrationBuilder.RenameColumn(
                name: "WeedImpurityBase",
                table: "ProductionBatch",
                newName: "WeedinessBase");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionBatches_RegisterId",
                table: "ProductionBatch",
                newName: "IX_ProductionBatch_RegisterId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductionBatches_LaboratoryCardId",
                table: "ProductionBatch",
                newName: "IX_ProductionBatch_LaboratoryCardId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "TechnologicalOperations",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CompletionReportId",
                table: "TechnologicalOperations",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Roles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "WasteReg",
                table: "Registers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShrinkageReg",
                table: "Registers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegisterNumber",
                table: "Registers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<int>(
                name: "PhysicalWeightReg",
                table: "Registers",
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
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "AccWeightReg",
                table: "Registers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "QuantityesDryingReg",
                table: "Registers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "PriceLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "Product",
                table: "PriceLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleNumber",
                table: "OutputInvoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ProductCategory",
                table: "OutputInvoices",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "OutputInvoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "OutInvNumber",
                table: "OutputInvoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "SpecialNotes",
                table: "LaboratoryCards",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "LabCardNumber",
                table: "LaboratoryCards",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "InputInvoices",
                type: "bit",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsFinalized",
                table: "CompletionReports",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PhysicalWeightReport",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "QuantityesDrying",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "Waste",
                table: "ProductionBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Shrinkage",
                table: "ProductionBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RegisterId",
                table: "ProductionBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AccountWeight",
                table: "ProductionBatch",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductionBatch",
                table: "ProductionBatch",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "DepotItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepotItemId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotItemCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotItemCategories_DepotItems_DepotItemId",
                        column: x => x.DepotItemId,
                        principalTable: "DepotItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PricesByOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PriceListId = table.Column<int>(type: "int", nullable: false),
                    OperationPrice = table.Column<double>(type: "float", nullable: false),
                    OperationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesByOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesByOperation_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_LaboratoryCardId",
                table: "InputInvoices",
                column: "LaboratoryCardId",
                unique: true,
                filter: "[LaboratoryCardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItemCategories_DepotItemId",
                table: "DepotItemCategories",
                column: "DepotItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PricesByOperation_PriceListId",
                table: "PricesByOperation",
                column: "PriceListId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_CreatedById",
                table: "CompletionReports",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_LaboratoryCards_LaboratoryCardId",
                table: "InputInvoices",
                column: "LaboratoryCardId",
                principalTable: "LaboratoryCards",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LaboratoryCards_Employees_CreatedById",
                table: "LaboratoryCards",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_Employees_CreatedById",
                table: "OutputInvoices",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceLists_Employees_CreatedById",
                table: "PriceLists",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatch_LaboratoryCards_LaboratoryCardId",
                table: "ProductionBatch",
                column: "LaboratoryCardId",
                principalTable: "LaboratoryCards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductionBatch_Registers_RegisterId",
                table: "ProductionBatch",
                column: "RegisterId",
                principalTable: "Registers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registers_Employees_CreatedById",
                table: "Registers",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                table: "TechnologicalOperations",
                column: "CompletionReportId",
                principalTable: "CompletionReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
