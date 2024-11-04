using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class modifyCompletionReportOperationOnDeleteDeleteBehaviorCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReportOperations_CompletionReports_CompletionReportId",
                table: "CompletionReportOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_CompletionReports_CompletionReportId",
                table: "CompletionReportOperations",
                column: "CompletionReportId",
                principalTable: "CompletionReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompletionReportOperations_CompletionReports_CompletionReportId",
                table: "CompletionReportOperations");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_CompletionReports_CompletionReportId",
                table: "CompletionReportOperations",
                column: "CompletionReportId",
                principalTable: "CompletionReports",
                principalColumn: "Id");
        }
    }
}
