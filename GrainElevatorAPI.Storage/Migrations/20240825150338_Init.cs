using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductTitles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTitles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DepotItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTitleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotItems_ProductTitles_ProductTitleId",
                        column: x => x.ProductTitleId,
                        principalTable: "ProductTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DepotItems_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppDefects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppDefects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppDefects_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LaboratoryCards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabCardNumber = table.Column<int>(type: "int", nullable: false),
                    Weediness = table.Column<double>(type: "float", nullable: false),
                    Moisture = table.Column<double>(type: "float", nullable: false),
                    GrainImpurity = table.Column<double>(type: "float", nullable: true),
                    SpecialNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsProduction = table.Column<bool>(type: "bit", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
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
                });

            migrationBuilder.CreateTable(
                name: "PriceLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedByInt = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PriceLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PriceLists_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DepotItemCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<int>(type: "int", nullable: false),
                    DepotItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DepotItemCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DepotItemCategories_DepotItems_DepotItemId",
                        column: x => x.DepotItemId,
                        principalTable: "DepotItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutputInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OutInvNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShipmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductCategory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductWeight = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTitleId = table.Column<int>(type: "int", nullable: false),
                    DepotItemId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutputInvoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_DepotItems_DepotItemId",
                        column: x => x.DepotItemId,
                        principalTable: "DepotItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OutputInvoices_ProductTitles_ProductTitleId",
                        column: x => x.ProductTitleId,
                        principalTable: "ProductTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OutputInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InputInvoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VehicleNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhysicalWeight = table.Column<int>(type: "int", nullable: false),
                    LaboratoryCardId = table.Column<int>(type: "int", nullable: true),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTitleId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
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
                        name: "FK_InputInvoices_LaboratoryCards_LaboratoryCardId",
                        column: x => x.LaboratoryCardId,
                        principalTable: "LaboratoryCards",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_InputInvoices_ProductTitles_ProductTitleId",
                        column: x => x.ProductTitleId,
                        principalTable: "ProductTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InputInvoices_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CompletionReports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportNumber = table.Column<int>(type: "int", nullable: false),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuantityesDrying = table.Column<double>(type: "float", nullable: false),
                    PhysicalWeightReport = table.Column<double>(type: "float", nullable: false),
                    IsFinalized = table.Column<bool>(type: "bit", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTitleId = table.Column<int>(type: "int", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompletionReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompletionReports_Employees_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReports_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CompletionReports_ProductTitles_ProductTitleId",
                        column: x => x.ProductTitleId,
                        principalTable: "ProductTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CompletionReports_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PricesByOperation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OperationTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OperationPrice = table.Column<double>(type: "float", nullable: false),
                    PriceListId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricesByOperation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PricesByOperation_PriceLists_PriceListId",
                        column: x => x.PriceListId,
                        principalTable: "PriceLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RegisterNumber = table.Column<int>(type: "int", nullable: false),
                    ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhysicalWeightReg = table.Column<int>(type: "int", nullable: false),
                    ShrinkageReg = table.Column<int>(type: "int", nullable: false),
                    WasteReg = table.Column<int>(type: "int", nullable: false),
                    AccWeightReg = table.Column<int>(type: "int", nullable: false),
                    QuantityesDryingReg = table.Column<double>(type: "float", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: false),
                    ProductTitleId = table.Column<int>(type: "int", nullable: false),
                    CompletionReportId = table.Column<int>(type: "int", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: true)
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Registers_ProductTitles_ProductTitleId",
                        column: x => x.ProductTitleId,
                        principalTable: "ProductTitles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Registers_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TechnologicalOperations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    TotalCost = table.Column<double>(type: "float", nullable: false),
                    CompletionReportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnologicalOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnologicalOperations_CompletionReports_CompletionReportId",
                        column: x => x.CompletionReportId,
                        principalTable: "CompletionReports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductionBatch",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeedinessBase = table.Column<double>(type: "float", nullable: false),
                    MoistureBase = table.Column<double>(type: "float", nullable: false),
                    Waste = table.Column<int>(type: "int", nullable: false),
                    Shrinkage = table.Column<int>(type: "int", nullable: false),
                    AccountWeight = table.Column<int>(type: "int", nullable: false),
                    LaboratoryCardId = table.Column<int>(type: "int", nullable: false),
                    RegisterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductionBatch_LaboratoryCards_LaboratoryCardId",
                        column: x => x.LaboratoryCardId,
                        principalTable: "LaboratoryCards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductionBatch_Registers_RegisterId",
                        column: x => x.RegisterId,
                        principalTable: "Registers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppDefects_CreatedById",
                table: "AppDefects",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_CreatedById",
                table: "CompletionReports",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_PriceListId",
                table: "CompletionReports",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_ProductTitleId",
                table: "CompletionReports",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_CompletionReports_SupplierId",
                table: "CompletionReports",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItemCategories_DepotItemId",
                table: "DepotItemCategories",
                column: "DepotItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_ProductTitleId",
                table: "DepotItems",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_DepotItems_SupplierId",
                table: "DepotItems",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_CreatedById",
                table: "InputInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_LaboratoryCardId",
                table: "InputInvoices",
                column: "LaboratoryCardId",
                unique: true,
                filter: "[LaboratoryCardId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_ProductTitleId",
                table: "InputInvoices",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_InputInvoices_SupplierId",
                table: "InputInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_LaboratoryCards_CreatedById",
                table: "LaboratoryCards",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_CreatedById",
                table: "OutputInvoices",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_DepotItemId",
                table: "OutputInvoices",
                column: "DepotItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_ProductTitleId",
                table: "OutputInvoices",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_OutputInvoices_SupplierId",
                table: "OutputInvoices",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceLists_CreatedById",
                table: "PriceLists",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_PricesByOperation_PriceListId",
                table: "PricesByOperation",
                column: "PriceListId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatch_LaboratoryCardId",
                table: "ProductionBatch",
                column: "LaboratoryCardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductionBatch_RegisterId",
                table: "ProductionBatch",
                column: "RegisterId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CompletionReportId",
                table: "Registers",
                column: "CompletionReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_CreatedById",
                table: "Registers",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_ProductTitleId",
                table: "Registers",
                column: "ProductTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_SupplierId",
                table: "Registers",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_TechnologicalOperations_CompletionReportId",
                table: "TechnologicalOperations",
                column: "CompletionReportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppDefects");

            migrationBuilder.DropTable(
                name: "DepotItemCategories");

            migrationBuilder.DropTable(
                name: "InputInvoices");

            migrationBuilder.DropTable(
                name: "OutputInvoices");

            migrationBuilder.DropTable(
                name: "PricesByOperation");

            migrationBuilder.DropTable(
                name: "ProductionBatch");

            migrationBuilder.DropTable(
                name: "TechnologicalOperations");

            migrationBuilder.DropTable(
                name: "DepotItems");

            migrationBuilder.DropTable(
                name: "LaboratoryCards");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "CompletionReports");

            migrationBuilder.DropTable(
                name: "PriceLists");

            migrationBuilder.DropTable(
                name: "ProductTitles");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
