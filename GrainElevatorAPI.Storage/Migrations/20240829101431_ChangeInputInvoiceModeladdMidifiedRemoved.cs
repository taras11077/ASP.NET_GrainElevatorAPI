using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInputInvoiceModeladdMidifiedRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateddAt",
                table: "InputInvoices",
                newName: "CreatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleNumber",
                table: "InputInvoices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedById",
                table: "InputInvoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "Removed",
                table: "InputInvoices",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RemovedAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RemovedById",
                table: "InputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_RemovedById",
                table: "InputInvoices",
                column: "RemovedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_RemovedById",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "Removed",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "RemovedAt",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "RemovedById",
                table: "InputInvoices");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "InputInvoices",
                newName: "CreateddAt");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleNumber",
                table: "InputInvoices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ModifiedById",
                table: "InputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifiedAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
