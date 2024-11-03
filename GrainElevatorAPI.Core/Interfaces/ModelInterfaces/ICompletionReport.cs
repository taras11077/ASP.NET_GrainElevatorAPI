using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReport
{
    int Id { get; set; }
    string ReportNumber { get; set; }
    DateTime ReportDate { get; set; }
    int? PhysicalWeightReport { get; set; }
    int? QuantitiesDryingReport { get; set; }
    int? ShrinkageReport { get; set; }
    int? WasteReport { get; set; }
    int? AccWeightReport { get; set; }
    
    decimal TotalCost { get; set; }
    bool? IsFinalized { get; set; }
    
    
    ICollection<InvoiceRegister> Registers { get; set; }
    ICollection<CompletionReportOperation> CompletionReportOperations { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    int? PriceListId { get; set; }
}