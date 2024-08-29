using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInputInvoiceModeladdRestore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices");

            migrationBuilder.AddColumn<int>(
                name: "RestoreById",
                table: "InputInvoices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RestoredAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_RestoreById",
                table: "InputInvoices",
                column: "RestoreById");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_RestoreById",
                table: "InputInvoices",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_RestoreById",
                table: "InputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_RestoreById",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "RestoreById",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "RestoredAt",
                table: "InputInvoices");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_RemovedById",
                table: "InputInvoices",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
