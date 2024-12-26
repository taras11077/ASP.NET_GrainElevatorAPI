using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInvoiceRegisterService
{
    Task<InvoiceRegister> CreateInvoiceRegisterAsync(
        string registerNumber,
        DateTime arrivalDate,
        string supplierTitle, 
        string productTitle, 
        double weedImpurityBase,
        double moistureBase,
        int createdById,
        CancellationToken cancellationToken);
    Task<InvoiceRegister> GetInvoiceRegisterByIdAsync(int id, CancellationToken cancellationToken);
    Task<InvoiceRegister> UpdateInvoiceRegisterAsync(
        int id, 
        string? registerNumber, 
        double? weedImpurityBase, 
        double? moistureBase, 
        int modifiedById, 
        CancellationToken cancellationToken);
    Task<InvoiceRegister> SoftDeleteInvoiceRegisterAsync(InvoiceRegister register, int removedById, CancellationToken cancellationToken);
    Task<InvoiceRegister> RestoreRemovedInvoiceRegisterAsync(InvoiceRegister register, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteInvoiceRegisterAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> GetInvoiceRegistersAsync(int page, int size, CancellationToken cancellationToken);

    Task<(IEnumerable<InvoiceRegister>, int)> SearchInvoiceRegistersAsync(
        string? registerNumber,
        DateTime? arrivalDate,
        int? physicalWeightReg,
        int? shrinkageReg,
        int? wasteReg,
        int? accWeightReg,
        double? weedImpurityBase,
        double? moistureBase,
        string? supplierTitle,
        string? productTitle,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
}