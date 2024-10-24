using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInputInvoiceService
{
    Task<InputInvoice> AddInputInvoiceAsync(InputInvoice inputInvoice, int createdById);
    Task<InputInvoice> GetInputInvoiceByIdAsync(int id);
    
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
    Task<InputInvoice> UpdateInputInvoiceAsync(InputInvoice inputInvoice, int modifiedById);
    
    Task<InputInvoice> SoftDeleteInputInvoiceAsync(InputInvoice inputInvoice, int removedById);
    Task<InputInvoice> RestoreRemovedInputInvoiceAsync(InputInvoice inputInvoice, int restoredById);
    Task<bool> DeleteInputInvoiceAsync(int id);
    
}