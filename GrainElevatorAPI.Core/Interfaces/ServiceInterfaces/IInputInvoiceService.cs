using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> CreateInputInvoiceAsync(string invoiceNumber, string supplierTitle, string productTitle, int createdById, CancellationToken cancellationToken);
    Task<InputInvoice> GetInputInvoiceByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<IEnumerable<InputInvoice>> GetInputInvoices(int page, int size, CancellationToken cancellationToken);

    Task<IEnumerable<InputInvoice>> SearchInputInvoices(int? id,
        string? invoiceNumber,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size, 
        CancellationToken cancellationToken);
    Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById, CancellationToken cancellationToken);
    
    Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById, CancellationToken cancellationToken);
    Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteInputInvoiceAsync(int id, CancellationToken cancellationToken);
    
}