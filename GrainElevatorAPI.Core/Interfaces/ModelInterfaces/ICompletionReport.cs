using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReport
{
    int Id { get; set; }
    string ReportNumber { get; set; }
    DateTime ReportDate { get; set; }
    double? ReportQuantitiesDrying { get; set; }
    double? ReportPhysicalWeight { get; set; }
    bool? IsFinalized { get; set; }
    
    ICollection<InvoiceRegister> Registers { get; set; }
    ICollection<CompletionReportOperation> CompletionReportItems { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    int? PriceListId { get; set; }
}