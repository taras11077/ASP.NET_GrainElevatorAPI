using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class renameTableTechnologicalOperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TechnologicalOperations");

            migrationBuilder.CreateTable(
                name: "CompletionReportItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnologicalOperation = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompletionReportItems");

            migrationBuilder.CreateTable(
                name: "TechnologicalOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RemovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false)
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
        }
    }
}
