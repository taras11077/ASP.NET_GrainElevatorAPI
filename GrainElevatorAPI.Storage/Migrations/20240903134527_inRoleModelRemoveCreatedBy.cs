using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class inRoleModelRemoveCreatedBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_CreatedById",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Employees_CreatedById",
                table: "Roles",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
