﻿using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IOutputInvoiceService
{
    Task<OutputInvoice> CreateOutputInvoiceAsync(string invoiceNumber,
        string vehicleNumber,
        int supplierId,
        int productId,
        string productCategory,
        int productWeight,
        int createdById, 
        CancellationToken cancellationToken);
    Task<OutputInvoice> GetOutputInvoiceByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<OutputInvoice>> GetOutputInvoices(int page, int size, CancellationToken cancellationToken);

    Task<IEnumerable<OutputInvoice>> SearchOutputInvoices(int? id,
        string? invoiceNumber,
        DateTime? shipmentDate,
        string? vehicleNumber,
        int? supplierId,
        int? productId,
        string productCategory,
        int? productWeight,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size, 
        CancellationToken cancellationToken);
    
    Task<OutputInvoice> UpdateOutputInvoiceAsync(OutputInvoice outputInvoice, int modifiedById, CancellationToken cancellationToken);
    
    Task<OutputInvoice> SoftDeleteOutputInvoiceAsync(OutputInvoice outputInvoice, int removedById, CancellationToken cancellationToken);
    Task<OutputInvoice> RestoreRemovedOutputInvoiceAsync(OutputInvoice outputInvoice, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteOutputInvoiceAsync(int id, CancellationToken cancellationToken);

}