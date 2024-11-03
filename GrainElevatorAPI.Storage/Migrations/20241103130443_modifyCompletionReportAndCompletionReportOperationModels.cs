using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class modifyCompletionReportAndCompletionReportOperationModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ReportPhysicalWeight",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ReportQuantitiesDrying",
                table: "CompletionReports");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "CompletionReports",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<int>(
                name: "AccWeightReport",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhysicalWeightReport",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QuantitiesDryingReport",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShrinkageReport",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WasteReport",
                table: "CompletionReports",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "CompletionReportOperations",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddColumn<decimal>(
                name: "OperationCost",
                table: "CompletionReportOperations",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccWeightReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "PhysicalWeightReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "QuantitiesDryingReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "ShrinkageReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "WasteReport",
                table: "CompletionReports");

            migrationBuilder.DropColumn(
                name: "OperationCost",
                table: "CompletionReportOperations");

            migrationBuilder.AlterColumn<double>(
                name: "TotalCost",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "CompletionReports",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReportPhysicalWeight",
                table: "CompletionReports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ReportQuantitiesDrying",
                table: "CompletionReports",
                type: "float",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "CompletionReportOperations",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
