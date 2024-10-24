using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Roles",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Employees",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "ModifiedAt", "ModifiedById", "RemovedAt", "RemovedById", "RestoreById", "RestoredAt", "Title" },
                values: new object[] { 1, new DateTime(2024, 10, 24, 13, 43, 4, 390, DateTimeKind.Utc).AddTicks(1184), null, null, null, null, null, null, null, "Admin" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "BirthDate", "City", "Country", "CreatedAt", "CreatedById", "Email", "FirstName", "Gender", "LastName", "LastSeenOnline", "ModifiedAt", "ModifiedById", "PasswordHash", "Phone", "RemovedAt", "RemovedById", "RestoreById", "RestoredAt", "RoleId" },
                values: new object[] { 1, null, null, null, new DateTime(2024, 10, 24, 13, 43, 4, 501, DateTimeKind.Utc).AddTicks(7644), null, "admin@example.com", null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "$2a$11$IRJYF0ChvbC/k45Ku7Uii.e5sqoqp0uqKqraqcYR7duH47Z.QTuW.", null, null, null, null, null, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CreatedById",
                table: "Employees",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
