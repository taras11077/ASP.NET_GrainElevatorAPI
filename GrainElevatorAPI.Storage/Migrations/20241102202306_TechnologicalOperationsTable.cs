using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class TechnologicalOperationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.DropIndex(
                name: "IX_TechnologicalOperations_CompletionReportId",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "CompletionReportId",
                table: "TechnologicalOperations");

            migrationBuilder.DropColumn(
                name: "OperationName",
                table: "PriceListItems");

            migrationBuilder.RenameColumn(
                name: "OperationName",
                table: "TechnologicalOperations",
                newName: "Title");

            migrationBuilder.AddColumn<int>(
                name: "TechnologicalOperationId",
                table: "PriceListItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ReportNumber",
                table: "CompletionReports",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 9);

            migrationBuilder.CreateTable(
                name: "CompletionReportOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnologicalOperationId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_CompletionReportOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_CompletionReports_CompletionReportId",
                        column: x => x.CompletionReportId,
                        principalTable: "CompletionReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReportOperations_TechnologicalOperations_TechnologicalOperationId",
                        column: x => x.TechnologicalOperationId,
                        principalTable: "TechnologicalOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_TechnologicalOperationId",
                table: "PriceListItems",
                column: "TechnologicalOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_CompletionReportId",
                table: "CompletionReportOperations",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_CreatedById",
                table: "CompletionReportOperations",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_ModifiedById",
                table: "CompletionReportOperations",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_RemovedById",
                table: "CompletionReportOperations",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_RestoreById",
                table: "CompletionReportOperations",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReportOperations_TechnologicalOperationId",
                table: "CompletionReportOperations",
                column: "TechnologicalOperationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceListItems_TechnologicalOperations_TechnologicalOperationId",
                table: "PriceListItems",
                column: "TechnologicalOperationId",
                principalTable: "TechnologicalOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_CreatedById",
                table: "TechnologicalOperations",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_ModifiedById",
                table: "TechnologicalOperations",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_RemovedById",
                table: "TechnologicalOperations",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TechnologicalOperations_Employees_RestoreById",
                table: "TechnologicalOperations",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceListItems_TechnologicalOperations_TechnologicalOperationId",
                table: "PriceListItems");

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
                name: "CompletionReportOperations");

            migrationBuilder.DropIndex(
                name: "IX_PriceListItems_TechnologicalOperationId",
                table: "PriceListItems");

            migrationBuilder.DropColumn(
                name: "TechnologicalOperationId",
                table: "PriceListItems");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "TechnologicalOperations",
                newName: "OperationName");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "TechnologicalOperations",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "CompletionReportId",
                table: "TechnologicalOperations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OperationName",
                table: "PriceListItems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ReportNumber",
                table: "CompletionReports",
                type: "int",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9);

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_CompletionReportId",
                table: "TechnologicalOperations",
                column: "CompletionReportId");

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
    }
}
