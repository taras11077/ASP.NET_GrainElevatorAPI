using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReport
{
    int Id { get; set; }
    string ReportNumber { get; set; }
    DateTime ReportDate { get; set; }
    double? PhysicalWeightReport { get; set; }
    double? QuantitiesDryingReport { get; set; }
    double? ShrinkageReport { get; set; }
    double? WasteReport { get; set; }
    double? AccWeightReport { get; set; }
    
    decimal TotalCost { get; set; }
    bool? IsFinalized { get; set; }
    
    
    ICollection<InvoiceRegister> Registers { get; set; }
    ICollection<CompletionReportOperation> CompletionReportOperations { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    int? PriceListId { get; set; }
}