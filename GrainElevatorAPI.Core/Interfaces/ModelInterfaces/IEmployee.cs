using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IEmployee
{
    int Id { get; set; }
    string? FirstName { get; set; }
    string? LastName { get; set; }
    DateTime? BirthDate { get; set; }
    string Email { get; set; }
    string? Phone { get; set; }
    string? Gender { get; set; }
    string? City { get; set; }
    string? Country { get; set; }
    string PasswordHash { get; set; }
    int RoleId { get; set; }
    DateTime LastSeenOnline { get; set; }

    
    ICollection<Role> Roles { get; set; }
    ICollection<Supplier> Suppliers { get; set; }
    ICollection<Product> Products { get; set; }
    ICollection<InputInvoice> InputInvoices { get; set; }
    ICollection<LaboratoryCard> LaboratoryCards { get; set; }
    ICollection<ProductionBatch> ProductionBatches { get; set; }
    ICollection<InvoiceRegister> Registers { get; set; }
    ICollection<CompletionReportOperation> CompletionReportItems { get; set; }
    ICollection<CompletionReport> CompletionReports { get; set; }
    ICollection<PriceListItem> PriceListItems { get; set; }
    ICollection<PriceList> PriceLists { get; set; }
    ICollection<WarehouseUnit> WarehouseUnits { get; set; }
    ICollection<WarehouseProductCategory> WarehouseProductCategories { get; set; }
    ICollection<OutputInvoice> OutputInvoices { get; set; }
}