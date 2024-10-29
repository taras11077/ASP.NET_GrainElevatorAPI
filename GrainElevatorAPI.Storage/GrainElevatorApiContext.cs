﻿using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.Core.Models.Base;
using GrainElevatorAPI.Core.Security;
using Microsoft.EntityFrameworkCore;

namespace GrainElevator.Storage;

public class GrainElevatorApiContext : DbContext
{
    public GrainElevatorApiContext(DbContextOptions<GrainElevatorApiContext> options) : base(options)
    {
        
    }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    // {
    //     optionsBuilder.UseSqlServer(
    //         "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=grainElevatorAPI_db;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
    // }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Product> Products { get; set; }
    
    public DbSet<InputInvoice> InputInvoices { get; set; }
    public DbSet<LaboratoryCard> LaboratoryCards { get; set; }
    
    public DbSet<ProductionBatch> ProductionBatches { get; set; }
    public DbSet<InvoiceRegister> Registers { get; set; }
    
    public DbSet<CompletionReportItem> CompletionReportItems { get; set; }
    public DbSet<PriceListItem> PriceListItems { get; set; }
    public DbSet<ProductionPriceList> PriceLists { get; set; }
    public DbSet<CompletionReport> CompletionReports { get; set; }
    
    public DbSet<WarehouseUnit> WarehouseUnits { get; set; }
    public DbSet<WarehouseProductCategory> WarehouseProductCategories { get; set; }
    public DbSet<OutputInvoice> OutputInvoices { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // // створення ролі
        // var adminRole = new Role
        // {
        //     Title = "Admin",
        //     CreatedAt = DateTime.UtcNow,
        // };
        // modelBuilder.Entity<Role>().HasData(adminRole);
        //
        // // створення адміна
        // var admin = new Employee
        // {
        //     Email = "admin@example.com",
        //     PasswordHash = PasswordHasher.HashPassword("Admin@123"),
        //     CreatedAt = DateTime.UtcNow,
        //     RoleId = 1
        // };
        // modelBuilder.Entity<Employee>().HasData(admin);
        
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.CreatedBy)
            .WithMany() // Обратная связь, если нет коллекции
            .HasForeignKey(e => e.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.ModifiedBy)
            .WithMany()
            .HasForeignKey(e => e.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.RemovedBy)
            .WithMany()
            .HasForeignKey(e => e.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.RestoreBy)
            .WithMany()
            .HasForeignKey(e => e.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<Role>()
            .HasOne(r => r.CreatedBy)
            .WithMany()
            .HasForeignKey(r => r.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Role>()
            .HasOne(r =>r.ModifiedBy)
            .WithMany(e => e.Roles)
            .HasForeignKey(r => r.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Role>()
            .HasOne(r => r.RemovedBy)
            .WithMany()
            .HasForeignKey(r => r.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Role>()
            .HasOne(r => r.RestoreBy)
            .WithMany()
            .HasForeignKey(r => r.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.CreatedBy)
            .WithMany(e => e.Suppliers)
            .HasForeignKey(s => s.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Supplier>()
            .HasOne(s =>s.ModifiedBy)
            .WithMany()
            .HasForeignKey(s => s.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.RemovedBy)
            .WithMany()
            .HasForeignKey(s => s.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.RestoreBy)
            .WithMany()
            .HasForeignKey(s => s.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.CreatedBy)
            .WithMany(e => e.Products)
            .HasForeignKey(p => p.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.ModifiedBy)
            .WithMany()
            .HasForeignKey(p => p.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.RemovedBy)
            .WithMany()
            .HasForeignKey(p => p.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.RestoreBy)
            .WithMany()
            .HasForeignKey(p => p.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<InputInvoice>()
            .HasOne(ii => ii.CreatedBy)
            .WithMany(e => e.InputInvoices)
            .HasForeignKey(ii => ii.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InputInvoice>()
            .HasOne(ii => ii.ModifiedBy)
            .WithMany()
            .HasForeignKey(ii => ii.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InputInvoice>()
            .HasOne(ii => ii.RemovedBy)
            .WithMany()
            .HasForeignKey(ii => ii.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InputInvoice>()
            .HasOne(ii => ii.RestoreBy)
            .WithMany()
            .HasForeignKey(ii => ii.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<LaboratoryCard>()
            .HasOne(lc => lc.InputInvoice)
            .WithOne(ii => ii.LaboratoryCard)
            .HasForeignKey<LaboratoryCard>(lc => lc.InputInvoiceId)
            .OnDelete(DeleteBehavior.Restrict);;
        
        modelBuilder.Entity<LaboratoryCard>()
            .HasOne(lc => lc.CreatedBy)
            .WithMany(e => e.LaboratoryCards)
            .HasForeignKey(lc => lc.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LaboratoryCard>()
            .HasOne(lc => lc.ModifiedBy)
            .WithMany()
            .HasForeignKey(lc => lc.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<LaboratoryCard>()
            .HasOne(lc => lc.RemovedBy)
            .WithMany()
            .HasForeignKey(lc => lc.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<LaboratoryCard>()
            .HasOne(lc => lc.RestoreBy)
            .WithMany()
            .HasForeignKey(lc => lc.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.LaboratoryCard)
            .WithOne(lc => lc.ProductionBatch)
            .HasForeignKey<ProductionBatch>(pb => pb.LaboratoryCardId)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.Register)
            .WithMany(r => r.ProductionBatches)
            .HasForeignKey(pb => pb.RegisterId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.CreatedBy)
            .WithMany(e => e.ProductionBatches)
            .HasForeignKey(pb => pb.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.ModifiedBy)
            .WithMany()
            .HasForeignKey(pb => pb.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.RemovedBy)
            .WithMany()
            .HasForeignKey(pb => pb.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductionBatch>()
            .HasOne(pb => pb.RestoreBy)
            .WithMany()
            .HasForeignKey(pb => pb.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<InvoiceRegister>()
            .HasOne(r => r.CreatedBy)
            .WithMany(e => e.Registers)
            .HasForeignKey(r => r.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<InvoiceRegister>()
            .HasOne(r => r.ModifiedBy)
            .WithMany()
            .HasForeignKey(r => r.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRegister>()
            .HasOne(r => r.RemovedBy)
            .WithMany()
            .HasForeignKey(r => r.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<InvoiceRegister>()
            .HasOne(r => r.RestoreBy)
            .WithMany()
            .HasForeignKey(r => r.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<CompletionReportItem>()
            .HasOne(to => to.CreatedBy)
            .WithMany(e => e.CompletionReportItems)
            .HasForeignKey(to => to.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompletionReportItem>()
            .HasOne(to => to.ModifiedBy)
            .WithMany()
            .HasForeignKey(to => to.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CompletionReportItem>()
            .HasOne(to => to.RemovedBy)
            .WithMany()
            .HasForeignKey(to => to.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CompletionReportItem>()
            .HasOne(to => to.RestoreBy)
            .WithMany()
            .HasForeignKey(to => to.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<CompletionReport>()
            .HasOne(cr => cr.CreatedBy)
            .WithMany(e => e.CompletionReports)
            .HasForeignKey(cr => cr.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CompletionReport>()
            .HasOne(cr => cr.ModifiedBy)
            .WithMany()
            .HasForeignKey(cr => cr.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CompletionReport>()
            .HasOne(cr => cr.RemovedBy)
            .WithMany()
            .HasForeignKey(cr => cr.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<CompletionReport>()
            .HasOne(cr => cr.RestoreBy)
            .WithMany()
            .HasForeignKey(cr => cr.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<PriceListItem>()
            .HasOne(pli => pli.CreatedBy)
            .WithMany(e => e.PriceListItems)
            .HasForeignKey(pli => pli.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PriceListItem>()
            .HasOne(pli => pli.ModifiedBy)
            .WithMany()
            .HasForeignKey(pli => pli.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PriceListItem>()
            .HasOne(pli => pli.RemovedBy)
            .WithMany()
            .HasForeignKey(pli => pli.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PriceListItem>()
            .HasOne(pli => pli.RestoreBy)
            .WithMany()
            .HasForeignKey(pli => pli.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        
        modelBuilder.Entity<ProductionPriceList>()
            .HasOne(pl => pl.CreatedBy)
            .WithMany(e => e.PriceLists)
            .HasForeignKey(pl => pl.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ProductionPriceList>()
            .HasOne(pl => pl.ModifiedBy)
            .WithMany()
            .HasForeignKey(pl => pl.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductionPriceList>()
            .HasOne(pl => pl.RemovedBy)
            .WithMany()
            .HasForeignKey(pl => pl.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<ProductionPriceList>()
            .HasOne(pl => pl.RestoreBy)
            .WithMany()
            .HasForeignKey(pl => pl.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        
        modelBuilder.Entity<WarehouseUnit>()
            .HasOne(di => di.CreatedBy)
            .WithMany(e => e.WarehouseUnits)
            .HasForeignKey(di => di.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WarehouseUnit>()
            .HasOne(di => di.ModifiedBy)
            .WithMany()
            .HasForeignKey(di => di.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<WarehouseUnit>()
            .HasOne(di => di.RemovedBy)
            .WithMany()
            .HasForeignKey(di => di.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<WarehouseUnit>()
            .HasOne(di => di.RestoreBy)
            .WithMany()
            .HasForeignKey(di => di.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        modelBuilder.Entity<WarehouseProductCategory>()
            .HasOne(wpc => wpc.WarehouseUnit)
            .WithMany(wu => wu.ProductCategories)
            .HasForeignKey(wpc => wpc.WarehouseUnitId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<WarehouseProductCategory>()
            .HasOne(dpc => dpc.CreatedBy)
            .WithMany(e => e.WarehouseProductCategories)
            .HasForeignKey(dpc => dpc.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WarehouseProductCategory>()
            .HasOne(dpc => dpc.ModifiedBy)
            .WithMany()
            .HasForeignKey(dpc => dpc.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<WarehouseProductCategory>()
            .HasOne(dpc => dpc.RemovedBy)
            .WithMany()
            .HasForeignKey(dpc => dpc.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<WarehouseProductCategory>()
            .HasOne(dpc => dpc.RestoreBy)
            .WithMany()
            .HasForeignKey(di => di.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        
        modelBuilder.Entity<OutputInvoice>()
            .HasOne(oi => oi.CreatedBy)
            .WithMany(e => e.OutputInvoices)
            .HasForeignKey(oi => oi.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OutputInvoice>()
            .HasOne(oi => oi.ModifiedBy)
            .WithMany()
            .HasForeignKey(ii => ii.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<OutputInvoice>()
            .HasOne(oi => oi.RemovedBy)
            .WithMany()
            .HasForeignKey(oi => oi.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<OutputInvoice>()
            .HasOne(oi => oi.RestoreBy)
            .WithMany()
            .HasForeignKey(oi => oi.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
    }
}