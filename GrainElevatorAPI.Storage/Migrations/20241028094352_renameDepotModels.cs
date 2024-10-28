using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class renameDepotModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_DepotItems_Products_ProductId",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotItems_Suppliers_SupplierId",
                table: "DepotItems");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_DepotItems_WarehouseUnitId",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_Employees_CreatedById",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_Employees_ModifiedById",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_Employees_RemovedById",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_DepotProductCategories_Employees_RestoreById",
                table: "DepotProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_DepotItems_WarehouseUnitId",
                table: "OutputInvoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepotProductCategories",
                table: "DepotProductCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DepotItems",
                table: "DepotItems");

            migrationBuilder.RenameTable(
                name: "DepotProductCategories",
                newName: "WarehouseProductCategories");

            migrationBuilder.RenameTable(
                name: "DepotItems",
                newName: "WarehouseUnits");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_WarehouseUnitId",
                table: "WarehouseProductCategories",
                newName: "IX_WarehouseProductCategories_WarehouseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_RestoreById",
                table: "WarehouseProductCategories",
                newName: "IX_WarehouseProductCategories_RestoreById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_RemovedById",
                table: "WarehouseProductCategories",
                newName: "IX_WarehouseProductCategories_RemovedById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_ModifiedById",
                table: "WarehouseProductCategories",
                newName: "IX_WarehouseProductCategories_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotProductCategories_CreatedById",
                table: "WarehouseProductCategories",
                newName: "IX_WarehouseProductCategories_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_SupplierId",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_RestoreById",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_RestoreById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_RemovedById",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_RemovedById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_ProductId",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_ModifiedById",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_DepotItems_CreatedById",
                table: "WarehouseUnits",
                newName: "IX_WarehouseUnits_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseProductCategories",
                table: "WarehouseProductCategories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WarehouseUnits",
                table: "WarehouseUnits",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_WarehouseUnits_WarehouseUnitId",
                table: "OutputInvoices",
                column: "WarehouseUnitId",
                principalTable: "WarehouseUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_Employees_CreatedById",
                table: "WarehouseProductCategories",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_Employees_ModifiedById",
                table: "WarehouseProductCategories",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_Employees_RemovedById",
                table: "WarehouseProductCategories",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_Employees_RestoreById",
                table: "WarehouseProductCategories",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories",
                column: "WarehouseUnitId",
                principalTable: "WarehouseUnits",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Employees_CreatedById",
                table: "WarehouseUnits",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Employees_ModifiedById",
                table: "WarehouseUnits",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Employees_RemovedById",
                table: "WarehouseUnits",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Employees_RestoreById",
                table: "WarehouseUnits",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Products_ProductId",
                table: "WarehouseUnits",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WarehouseUnits_Suppliers_SupplierId",
                table: "WarehouseUnits",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutputInvoices_WarehouseUnits_WarehouseUnitId",
                table: "OutputInvoices");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_Employees_CreatedById",
                table: "WarehouseProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_Employees_ModifiedById",
                table: "WarehouseProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_Employees_RemovedById",
                table: "WarehouseProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_Employees_RestoreById",
                table: "WarehouseProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                table: "WarehouseProductCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Employees_CreatedById",
                table: "WarehouseUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Employees_ModifiedById",
                table: "WarehouseUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Employees_RemovedById",
                table: "WarehouseUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Employees_RestoreById",
                table: "WarehouseUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Products_ProductId",
                table: "WarehouseUnits");

            migrationBuilder.DropForeignKey(
                name: "FK_WarehouseUnits_Suppliers_SupplierId",
                table: "WarehouseUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseUnits",
                table: "WarehouseUnits");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WarehouseProductCategories",
                table: "WarehouseProductCategories");

            migrationBuilder.RenameTable(
                name: "WarehouseUnits",
                newName: "DepotItems");

            migrationBuilder.RenameTable(
                name: "WarehouseProductCategories",
                newName: "DepotProductCategories");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_SupplierId",
                table: "DepotItems",
                newName: "IX_DepotItems_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_RestoreById",
                table: "DepotItems",
                newName: "IX_DepotItems_RestoreById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_RemovedById",
                table: "DepotItems",
                newName: "IX_DepotItems_RemovedById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_ProductId",
                table: "DepotItems",
                newName: "IX_DepotItems_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_ModifiedById",
                table: "DepotItems",
                newName: "IX_DepotItems_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseUnits_CreatedById",
                table: "DepotItems",
                newName: "IX_DepotItems_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseProductCategories_WarehouseUnitId",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_WarehouseUnitId");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseProductCategories_RestoreById",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_RestoreById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseProductCategories_RemovedById",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_RemovedById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseProductCategories_ModifiedById",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_WarehouseProductCategories_CreatedById",
                table: "DepotProductCategories",
                newName: "IX_DepotProductCategories_CreatedById");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepotItems",
                table: "DepotItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DepotProductCategories",
                table: "DepotProductCategories",
                column: "Id");

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
                name: "FK_DepotItems_Products_ProductId",
                table: "DepotItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotItems_Suppliers_SupplierId",
                table: "DepotItems",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_DepotItems_WarehouseUnitId",
                table: "DepotProductCategories",
                column: "WarehouseUnitId",
                principalTable: "DepotItems",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_Employees_CreatedById",
                table: "DepotProductCategories",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_Employees_ModifiedById",
                table: "DepotProductCategories",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_Employees_RemovedById",
                table: "DepotProductCategories",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DepotProductCategories_Employees_RestoreById",
                table: "DepotProductCategories",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OutputInvoices_DepotItems_WarehouseUnitId",
                table: "OutputInvoices",
                column: "WarehouseUnitId",
                principalTable: "DepotItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
