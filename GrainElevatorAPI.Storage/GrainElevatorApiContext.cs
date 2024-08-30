using GrainElevatorAPI.Core.Models;
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
    public DbSet<Register> Registers { get; set; }
    
    public DbSet<TechnologicalOperation> TechnologicalOperations { get; set; }
    public DbSet<PriceListItem> PriceListItems { get; set; }
    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<CompletionReport> CompletionReports { get; set; }
    
    public DbSet<DepotItem> DepotItems { get; set; }
    public DbSet<DepotProductCategory> DepotProductCategories { get; set; }
    public DbSet<OutputInvoice> OutputInvoices { get; set; }
    
    public DbSet<AppDefect> AppDefects { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Role>()
            .HasOne(r => r.CreatedBy)
            .WithMany(e => e.Roles)
            .HasForeignKey(r => r.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Role>()
            .HasOne(r =>r.ModifiedBy)
            .WithMany()
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
            .HasForeignKey(ii => ii.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Supplier>()
            .HasOne(s => s.RemovedBy)
            .WithMany()
            .HasForeignKey(ii => ii.RemovedById)
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
            .HasOne(l => l.InputInvoice)
            .WithOne(i => i.LaboratoryCard)
            .HasForeignKey<LaboratoryCard>(l => l.InputInvoiceId);
        
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
            .WithOne(l => l.ProductionBatch)
            .HasForeignKey<ProductionBatch>(pb => pb.LaboratoryCardId);
        
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
        
        
        
        modelBuilder.Entity<Register>()
            .HasOne(r => r.CreatedBy)
            .WithMany(e => e.Registers)
            .HasForeignKey(r => r.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Register>()
            .HasOne(r => r.ModifiedBy)
            .WithMany()
            .HasForeignKey(ii => ii.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Register>()
            .HasOne(r => r.RemovedBy)
            .WithMany()
            .HasForeignKey(r => r.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<Register>()
            .HasOne(r => r.RestoreBy)
            .WithMany()
            .HasForeignKey(r => r.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<TechnologicalOperation>()
            .HasOne(to => to.CreatedBy)
            .WithMany(e => e.TechnologicalOperations)
            .HasForeignKey(oi => oi.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<TechnologicalOperation>()
            .HasOne(to => to.ModifiedBy)
            .WithMany()
            .HasForeignKey(to => to.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TechnologicalOperation>()
            .HasOne(to => to.RemovedBy)
            .WithMany()
            .HasForeignKey(to => to.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<TechnologicalOperation>()
            .HasOne(to => to.RestoreBy)
            .WithMany()
            .HasForeignKey(to => to.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<CompletionReport>()
            .HasOne(cr => cr.CreatedBy)
            .WithMany(e => e.CompletionReports)
            .HasForeignKey(oi => oi.CreatedById)
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
        
        
        
        
        modelBuilder.Entity<PriceList>()
            .HasOne(pl => pl.CreatedBy)
            .WithMany(e => e.PriceLists)
            .HasForeignKey(pl => pl.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PriceList>()
            .HasOne(pl => pl.ModifiedBy)
            .WithMany()
            .HasForeignKey(pl => pl.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PriceList>()
            .HasOne(pl => pl.RemovedBy)
            .WithMany()
            .HasForeignKey(pl => pl.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PriceList>()
            .HasOne(pl => pl.RestoreBy)
            .WithMany()
            .HasForeignKey(pl => pl.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        
        modelBuilder.Entity<DepotItem>()
            .HasOne(di => di.CreatedBy)
            .WithMany(e => e.DepotItems)
            .HasForeignKey(di => di.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DepotItem>()
            .HasOne(di => di.ModifiedBy)
            .WithMany()
            .HasForeignKey(di => di.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DepotItem>()
            .HasOne(di => di.RemovedBy)
            .WithMany()
            .HasForeignKey(di => di.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DepotItem>()
            .HasOne(di => di.RestoreBy)
            .WithMany()
            .HasForeignKey(di => di.RestoreById)
            .OnDelete(DeleteBehavior.Restrict);
        
        
        
        modelBuilder.Entity<DepotProductCategory>()
            .HasOne(dpc => dpc.CreatedBy)
            .WithMany(e => e.DepotProductCategories)
            .HasForeignKey(dpc => dpc.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DepotProductCategory>()
            .HasOne(dpc => dpc.ModifiedBy)
            .WithMany()
            .HasForeignKey(dpc => dpc.ModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DepotProductCategory>()
            .HasOne(dpc => dpc.RemovedBy)
            .WithMany()
            .HasForeignKey(dpc => dpc.RemovedById)
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<DepotProductCategory>()
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