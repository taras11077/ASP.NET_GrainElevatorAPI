using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompletionReportOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TechnologicalOperationId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "double", nullable: false),
                    OperationCost = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionReportOperations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CompletionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ReportNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ReportDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PhysicalWeightReport = table.Column<double>(type: "double", nullable: true),
                    QuantitiesDryingReport = table.Column<double>(type: "double", nullable: true),
                    ShrinkageReport = table.Column<double>(type: "double", nullable: true),
                    WasteReport = table.Column<double>(type: "double", nullable: true),
                    AccWeightReport = table.Column<double>(type: "double", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    IsFinalized = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionReports", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Gender = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastSeenOnline = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Employees_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Suppliers_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Suppliers_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "TechnologicalOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsFinalized = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceLists_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceLists_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceLists_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceLists_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InputInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArrivalDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhysicalWeight = table.Column<int>(type: "int", nullable: false),
                    IsFinalized = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InputInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RegisterNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ArrivalDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    WeedImpurityBase = table.Column<double>(type: "double", nullable: false),
                    MoistureBase = table.Column<double>(type: "double", nullable: false),
                    PhysicalWeightReg = table.Column<int>(type: "int", nullable: true),
                    ShrinkageReg = table.Column<int>(type: "int", nullable: true),
                    WasteReg = table.Column<int>(type: "int", nullable: true),
                    AccWeightReg = table.Column<int>(type: "int", nullable: true),
                    QuantitiesDryingReg = table.Column<double>(type: "double", nullable: true),
                    IsFinalized = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Registers_CompletionReports_CompletionReportId",
                        column: x => x.CompletionReportId,
                        principalTable: "CompletionReports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registers_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WarehouseUnits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarehouseUnits_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PriceListItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OperationPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    TechnologicalOperationId = table.Column<int>(type: "int", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceListItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PriceListItems_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PriceListItems_TechnologicalOperations_TechnologicalOperatio~",
                        column: x => x.TechnologicalOperationId,
                        principalTable: "TechnologicalOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "LaboratoryCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LabCardNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    WeedImpurity = table.Column<double>(type: "double", nullable: false),
                    Moisture = table.Column<double>(type: "double", nullable: false),
                    GrainImpurity = table.Column<double>(type: "double", nullable: true),
                    SpecialNotes = table.Column<string>(type: "varchar(300)", maxLength: 300, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsProduction = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    IsFinalized = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    InputInvoiceId = table.Column<int>(type: "int", nullable: false),
                    ProductionBatchId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaboratoryCards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LaboratoryCards_InputInvoices_InputInvoiceId",
                        column: x => x.InputInvoiceId,
                        principalTable: "InputInvoices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "OutputInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    InvoiceNumber = table.Column<string>(type: "varchar(9)", maxLength: 9, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ShipmentDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    VehicleNumber = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductCategory = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProductWeight = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    WarehouseUnitId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_WarehouseUnits_WarehouseUnitId",
                        column: x => x.WarehouseUnitId,
                        principalTable: "WarehouseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "WarehouseProductCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Value = table.Column<int>(type: "int", nullable: true),
                    WarehouseUnitId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseProductCategories_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProductCategories_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProductCategories_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProductCategories_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProductCategories_WarehouseUnits_WarehouseUnitId",
                        column: x => x.WarehouseUnitId,
                        principalTable: "WarehouseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductionBatches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Waste = table.Column<int>(type: "int", nullable: true),
                    Shrinkage = table.Column<int>(type: "int", nullable: true),
                    AccountWeight = table.Column<int>(type: "int", nullable: true),
                    QuantitiesDrying = table.Column<double>(type: "double", nullable: true),
                    LaboratoryCardId = table.Column<int>(type: "int", nullable: false),
                    RegisterId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RemovedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    RestoredAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true),
                    ModifiedById = table.Column<int>(type: "int", nullable: true),
                    RemovedById = table.Column<int>(type: "int", nullable: true),
                    RestoreById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionBatches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_Employees_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_Employees_RemovedById",
                        column: x => x.RemovedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_Employees_RestoreById",
                        column: x => x.RestoreById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_LaboratoryCards_LaboratoryCardId",
                        column: x => x.LaboratoryCardId,
                        principalTable: "LaboratoryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatches_Registers_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "Registers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_CreatedById",
                table: "CompletionReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ModifiedById",
                table: "CompletionReports",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_PriceListId",
                table: "CompletionReports",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ProductId",
                table: "CompletionReports",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_RemovedById",
                table: "CompletionReports",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_RestoreById",
                table: "CompletionReports",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_SupplierId",
                table: "CompletionReports",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CreatedById",
                table: "Employees",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ModifiedById",
                table: "Employees",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RemovedById",
                table: "Employees",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RestoreById",
                table: "Employees",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_CreatedById",
                table: "InputInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ModifiedById",
                table: "InputInvoices",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ProductId",
                table: "InputInvoices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_RemovedById",
                table: "InputInvoices",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_RestoreById",
                table: "InputInvoices",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_SupplierId",
                table: "InputInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_CreatedById",
                table: "LaboratoryCards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_InputInvoiceId",
                table: "LaboratoryCards",
                column: "InputInvoiceId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_ModifiedById",
                table: "LaboratoryCards",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_RemovedById",
                table: "LaboratoryCards",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_RestoreById",
                table: "LaboratoryCards",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_CreatedById",
                table: "OutputInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ModifiedById",
                table: "OutputInvoices",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ProductId",
                table: "OutputInvoices",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_RemovedById",
                table: "OutputInvoices",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_RestoreById",
                table: "OutputInvoices",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_SupplierId",
                table: "OutputInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_WarehouseUnitId",
                table: "OutputInvoices",
                column: "WarehouseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_CreatedById",
                table: "PriceListItems",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_ModifiedById",
                table: "PriceListItems",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_PriceListId",
                table: "PriceListItems",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_RemovedById",
                table: "PriceListItems",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_RestoreById",
                table: "PriceListItems",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceListItems_TechnologicalOperationId",
                table: "PriceListItems",
                column: "TechnologicalOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_CreatedById",
                table: "PriceLists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_ModifiedById",
                table: "PriceLists",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_ProductId",
                table: "PriceLists",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_RemovedById",
                table: "PriceLists",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_RestoreById",
                table: "PriceLists",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_CreatedById",
                table: "ProductionBatches",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_LaboratoryCardId",
                table: "ProductionBatches",
                column: "LaboratoryCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_ModifiedById",
                table: "ProductionBatches",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_RegisterId",
                table: "ProductionBatches",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_RemovedById",
                table: "ProductionBatches",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatches_RestoreById",
                table: "ProductionBatches",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatedById",
                table: "Products",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ModifiedById",
                table: "Products",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RemovedById",
                table: "Products",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Products_RestoreById",
                table: "Products",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CompletionReportId",
                table: "Registers",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CreatedById",
                table: "Registers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ModifiedById",
                table: "Registers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ProductId",
                table: "Registers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_RemovedById",
                table: "Registers",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_RestoreById",
                table: "Registers",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_SupplierId",
                table: "Registers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_CreatedById",
                table: "Roles",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_ModifiedById",
                table: "Roles",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RemovedById",
                table: "Roles",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_RestoreById",
                table: "Roles",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_CreatedById",
                table: "Suppliers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_ModifiedById",
                table: "Suppliers",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_RemovedById",
                table: "Suppliers",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_RestoreById",
                table: "Suppliers",
                column: "RestoreById");

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

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductCategories_CreatedById",
                table: "WarehouseProductCategories",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductCategories_ModifiedById",
                table: "WarehouseProductCategories",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductCategories_RemovedById",
                table: "WarehouseProductCategories",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductCategories_RestoreById",
                table: "WarehouseProductCategories",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProductCategories_WarehouseUnitId",
                table: "WarehouseProductCategories",
                column: "WarehouseUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_CreatedById",
                table: "WarehouseUnits",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_ModifiedById",
                table: "WarehouseUnits",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_ProductId",
                table: "WarehouseUnits",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_RemovedById",
                table: "WarehouseUnits",
                column: "RemovedById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_RestoreById",
                table: "WarehouseUnits",
                column: "RestoreById");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseUnits_SupplierId",
                table: "WarehouseUnits",
                column: "SupplierId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_CompletionReports_CompletionRepor~",
                table: "CompletionReportOperations",
                column: "CompletionReportId",
                principalTable: "CompletionReports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_Employees_CreatedById",
                table: "CompletionReportOperations",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_Employees_ModifiedById",
                table: "CompletionReportOperations",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_Employees_RemovedById",
                table: "CompletionReportOperations",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_Employees_RestoreById",
                table: "CompletionReportOperations",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReportOperations_TechnologicalOperations_Technolog~",
                table: "CompletionReportOperations",
                column: "TechnologicalOperationId",
                principalTable: "TechnologicalOperations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_CreatedById",
                table: "CompletionReports",
                column: "CreatedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_ModifiedById",
                table: "CompletionReports",
                column: "ModifiedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_RemovedById",
                table: "CompletionReports",
                column: "RemovedById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Employees_RestoreById",
                table: "CompletionReports",
                column: "RestoreById",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_PriceLists_PriceListId",
                table: "CompletionReports",
                column: "PriceListId",
                principalTable: "PriceLists",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Products_ProductId",
                table: "CompletionReports",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompletionReports_Suppliers_SupplierId",
                table: "CompletionReports",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Roles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_CreatedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_ModifiedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_RemovedById",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Employees_RestoreById",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "CompletionReportOperations");

            migrationBuilder.DropTable(
                name: "OutputInvoices");

            migrationBuilder.DropTable(
                name: "PriceListItems");

            migrationBuilder.DropTable(
                name: "ProductionBatches");

            migrationBuilder.DropTable(
                name: "WarehouseProductCategories");

            migrationBuilder.DropTable(
                name: "TechnologicalOperations");

            migrationBuilder.DropTable(
                name: "LaboratoryCards");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "WarehouseUnits");

            migrationBuilder.DropTable(
                name: "InputInvoices");

            migrationBuilder.DropTable(
                name: "CompletionReports");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
