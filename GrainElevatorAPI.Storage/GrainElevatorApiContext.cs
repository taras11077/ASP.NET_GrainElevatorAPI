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
    public DbSet<ProductTitle> ProductTitles { get; set; }
    
    public DbSet<InputInvoice> InputInvoices { get; set; }
    public DbSet<LaboratoryCard> LaboratoryCards { get; set; }
    
    public DbSet<ProductionBatch> ProductionBatch { get; set; }
    public DbSet<Register> Registers { get; set; }
    
    public DbSet<TechnologicalOperation> TechnologicalOperations { get; set; }
    public DbSet<PriceByOperation> PricesByOperation { get; set; }
    public DbSet<PriceList> PriceLists { get; set; }
    public DbSet<CompletionReport> CompletionReports { get; set; }
    
    public DbSet<DepotItem> DepotItems { get; set; }
    public DbSet<ProductCategory> DepotItemCategories { get; set; }
    public DbSet<OutputInvoice> OutputInvoices { get; set; }
    
    public DbSet<AppDefect> AppDefects { get; set; }
    
}