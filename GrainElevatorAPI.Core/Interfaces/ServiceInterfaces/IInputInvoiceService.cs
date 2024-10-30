using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice, int createdById, CancellationToken cancellationToken);
    Task<InputInvoice> GetInputInvoiceByIdAsync(int id, CancellationToken cancellationToken);
    
    IEnumerable<InputInvoice> GetInputInvoices(int page, int size);

    IEnumerable<InputInvoice> SearchInputInvoices(int? id,
        string? invoiceNumber,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size);
    Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById, CancellationToken cancellationToken);
    
    Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById, CancellationToken cancellationToken);
    Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteInputInvoiceAsync(int id, CancellationToken cancellationToken);
    
}