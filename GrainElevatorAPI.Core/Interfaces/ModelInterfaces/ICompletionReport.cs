﻿using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface ICompletionReport
{
    int Id { get; set; }
    int ReportNumber { get; set; }
    DateTime ReportDate { get; set; }
    double? QuantitiesDrying { get; set; }
    double? ReportPhysicalWeight { get; set; }
    bool? IsFinalized { get; set; }
    
    ICollection<InvoiceRegister> Registers { get; set; }
    ICollection<CompletionReportItem> CompletionReportItems { get; set; }
    int SupplierId { get; set; }
    int ProductId { get; set; }
    int? PriceListId { get; set; }
}