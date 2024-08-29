using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInputInvoiceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_CreatedById",
                table: "InputInvoices");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateddAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedAt",
                table: "InputInvoices",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ModifiedById",
                table: "InputInvoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ModifiedById",
                table: "InputInvoices",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_CreatedById",
                table: "InputInvoices",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_ModifiedById",
                table: "InputInvoices",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_CreatedById",
                table: "InputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_InputInvoices_Employees_ModifiedById",
                table: "InputInvoices");

            migrationBuilder.DropIndex(
                name: "IX_InputInvoices_ModifiedById",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "CreateddAt",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                table: "InputInvoices");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                table: "InputInvoices");

            migrationBuilder.AddForeignKey(
                name: "FK_InputInvoices_Employees_CreatedById",
                table: "InputInvoices",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
