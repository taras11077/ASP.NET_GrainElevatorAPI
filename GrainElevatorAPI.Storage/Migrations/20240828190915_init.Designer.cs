﻿// <auto-generated />
using System;
using GrainElevator.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GrainElevator.Storage.Migrations
{
    [DbContext(typeof(GrainElevatorApiContext))]
    [Migration("20240828190915_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.AppDefect", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CompanyName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("AppDefects");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.CompletionReport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<bool>("IsFinalized")
                        .HasColumnType("bit");

                    b.Property<double>("PhysicalWeightReport")
                        .HasColumnType("float");

                    b.Property<int?>("PriceListId")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("QuantityesDrying")
                        .HasColumnType("float");

                    b.Property<DateTime>("ReportDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ReportNumber")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("PriceListId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("CompletionReports");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.DepotItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("DepotItems");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSeenOnline")
                        .HasColumnType("datetime2");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.InputInvoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("LaboratoryCardId")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalWeight")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("VehicleNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("LaboratoryCardId")
                        .IsUnique()
                        .HasFilter("[LaboratoryCardId] IS NOT NULL");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("InputInvoices");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.LaboratoryCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CreatedById")
                        .HasColumnType("int");

                    b.Property<double?>("GrainImpurity")
                        .HasColumnType("float");

                    b.Property<bool?>("IsProduction")
                        .HasColumnType("bit");

                    b.Property<int>("LabCardNumber")
                        .HasColumnType("int");

                    b.Property<double>("Moisture")
                        .HasColumnType("float");

                    b.Property<string>("SpecialNotes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Weediness")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("LaboratoryCards");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.OutputInvoice", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<int>("DepotItemId")
                        .HasColumnType("int");

                    b.Property<string>("OutInvNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProductCategory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("ProductWeight")
                        .HasColumnType("int");

                    b.Property<DateTime>("ShipmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<string>("VehicleNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.HasIndex("DepotItemId");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("OutputInvoices");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.PriceByOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("OperationPrice")
                        .HasColumnType("float");

                    b.Property<string>("OperationTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PriceListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PriceListId");

                    b.ToTable("PricesByOperation");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.PriceList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedByInt")
                        .HasColumnType("int");

                    b.Property<string>("Product")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CreatedById");

                    b.ToTable("PriceLists");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.ProductCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("DepotItemId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DepotItemId");

                    b.ToTable("DepotItemCategories");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.ProductionBatch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccountWeight")
                        .HasColumnType("int");

                    b.Property<int>("LaboratoryCardId")
                        .HasColumnType("int");

                    b.Property<double>("MoistureBase")
                        .HasColumnType("float");

                    b.Property<int>("RegisterId")
                        .HasColumnType("int");

                    b.Property<int>("Shrinkage")
                        .HasColumnType("int");

                    b.Property<int>("Waste")
                        .HasColumnType("int");

                    b.Property<double>("WeedinessBase")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("LaboratoryCardId")
                        .IsUnique();

                    b.HasIndex("RegisterId");

                    b.ToTable("ProductionBatch");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Register", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccWeightReg")
                        .HasColumnType("int");

                    b.Property<DateTime>("ArrivalDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CompletionReportId")
                        .HasColumnType("int");

                    b.Property<int?>("CreatedById")
                        .HasColumnType("int");

                    b.Property<int>("PhysicalWeightReg")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<double>("QuantityesDryingReg")
                        .HasColumnType("float");

                    b.Property<int>("RegisterNumber")
                        .HasColumnType("int");

                    b.Property<int>("ShrinkageReg")
                        .HasColumnType("int");

                    b.Property<int>("SupplierId")
                        .HasColumnType("int");

                    b.Property<int>("WasteReg")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompletionReportId");

                    b.HasIndex("CreatedById");

                    b.HasIndex("ProductId");

                    b.HasIndex("SupplierId");

                    b.ToTable("Registers");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Supplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Suppliers");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.TechnologicalOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<int>("CompletionReportId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("TotalCost")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("CompletionReportId");

                    b.ToTable("TechnologicalOperations");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.AppDefect", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany()
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.CompletionReport", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("CompletionReports")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.PriceList", "PriceList")
                        .WithMany("CompletionReports")
                        .HasForeignKey("PriceListId");

                    b.HasOne("GrainElevatorAPI.Core.Models.Product", "Product")
                        .WithMany("CompletionReports")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Supplier", "Supplier")
                        .WithMany("CompletionReports")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("PriceList");

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.DepotItem", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Product", "Product")
                        .WithMany("DepotItems")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Supplier", "Supplier")
                        .WithMany("DepotItems")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Employee", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.InputInvoice", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("InputInvoices")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.LaboratoryCard", "LaboratoryCard")
                        .WithOne("InputInvoice")
                        .HasForeignKey("GrainElevatorAPI.Core.Models.InputInvoice", "LaboratoryCardId");

                    b.HasOne("GrainElevatorAPI.Core.Models.Product", "Product")
                        .WithMany("InputInvoices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Supplier", "Supplier")
                        .WithMany("InputInvoices")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("LaboratoryCard");

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.LaboratoryCard", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("LaboratoryCards")
                        .HasForeignKey("CreatedById")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.OutputInvoice", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("OutputInvoices")
                        .HasForeignKey("CreatedById");

                    b.HasOne("GrainElevatorAPI.Core.Models.DepotItem", "DepotItem")
                        .WithMany("OutputInvoices")
                        .HasForeignKey("DepotItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Product", "Product")
                        .WithMany("OutputInvoices")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Supplier", "Supplier")
                        .WithMany("OutputInvoices")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedBy");

                    b.Navigation("DepotItem");

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.PriceByOperation", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.PriceList", "PriceList")
                        .WithMany("PriceByOperations")
                        .HasForeignKey("PriceListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PriceList");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.PriceList", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("PriceLists")
                        .HasForeignKey("CreatedById");

                    b.Navigation("CreatedBy");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.ProductCategory", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.DepotItem", "DepotItem")
                        .WithMany("ProductCategories")
                        .HasForeignKey("DepotItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DepotItem");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.ProductionBatch", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.LaboratoryCard", "LaboratoryCard")
                        .WithOne("ProductionBatch")
                        .HasForeignKey("GrainElevatorAPI.Core.Models.ProductionBatch", "LaboratoryCardId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Register", "Register")
                        .WithMany("ProductionBatches")
                        .HasForeignKey("RegisterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LaboratoryCard");

                    b.Navigation("Register");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Register", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.CompletionReport", "CompletionReport")
                        .WithMany("Registers")
                        .HasForeignKey("CompletionReportId");

                    b.HasOne("GrainElevatorAPI.Core.Models.Employee", "CreatedBy")
                        .WithMany("Registers")
                        .HasForeignKey("CreatedById");

                    b.HasOne("GrainElevatorAPI.Core.Models.Product", "Product")
                        .WithMany("Registers")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GrainElevatorAPI.Core.Models.Supplier", "Supplier")
                        .WithMany("Registers")
                        .HasForeignKey("SupplierId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompletionReport");

                    b.Navigation("CreatedBy");

                    b.Navigation("Product");

                    b.Navigation("Supplier");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.TechnologicalOperation", b =>
                {
                    b.HasOne("GrainElevatorAPI.Core.Models.CompletionReport", "CompletionReport")
                        .WithMany("TechnologicalOperations")
                        .HasForeignKey("CompletionReportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CompletionReport");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.CompletionReport", b =>
                {
                    b.Navigation("Registers");

                    b.Navigation("TechnologicalOperations");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.DepotItem", b =>
                {
                    b.Navigation("OutputInvoices");

                    b.Navigation("ProductCategories");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Employee", b =>
                {
                    b.Navigation("CompletionReports");

                    b.Navigation("InputInvoices");

                    b.Navigation("LaboratoryCards");

                    b.Navigation("OutputInvoices");

                    b.Navigation("PriceLists");

                    b.Navigation("Registers");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.LaboratoryCard", b =>
                {
                    b.Navigation("InputInvoice")
                        .IsRequired();

                    b.Navigation("ProductionBatch");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.PriceList", b =>
                {
                    b.Navigation("CompletionReports");

                    b.Navigation("PriceByOperations");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Product", b =>
                {
                    b.Navigation("CompletionReports");

                    b.Navigation("DepotItems");

                    b.Navigation("InputInvoices");

                    b.Navigation("OutputInvoices");

                    b.Navigation("Registers");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Register", b =>
                {
                    b.Navigation("ProductionBatches");
                });

            modelBuilder.Entity("GrainElevatorAPI.Core.Models.Supplier", b =>
                {
                    b.Navigation("CompletionReports");

                    b.Navigation("DepotItems");

                    b.Navigation("InputInvoices");

                    b.Navigation("OutputInvoices");

                    b.Navigation("Registers");
                });
#pragma warning restore 612, 618
        }
    }
}
