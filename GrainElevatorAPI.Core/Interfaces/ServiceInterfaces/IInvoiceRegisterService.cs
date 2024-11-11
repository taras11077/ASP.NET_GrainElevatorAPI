using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInvoiceRegisterService
{
    Task<InvoiceRegister> CreateInvoiceRegisterAsync(
        string registerNumber,
        int supplierId,
        int productId,
        double weedImpurityBase,
        double moistureBase,
        IEnumerable<int> laboratoryCardIds,
        int createdById,
        CancellationToken cancellationToken);
    Task<InvoiceRegister> GetInvoiceRegisterByIdAsync(int id, CancellationToken cancellationToken);
    Task<InvoiceRegister> UpdateInvoiceRegisterAsync(
        int id, 
        string? registerNumber, 
        double? weedImpurityBase, 
        double? moistureBase, 
        List<int>? laboratoryCardIds, 
        int modifiedById, 
        CancellationToken cancellationToken);
    Task<InvoiceRegister> SoftDeleteInvoiceRegisterAsync(InvoiceRegister register, int removedById, CancellationToken cancellationToken);
    Task<InvoiceRegister> RestoreRemovedInvoiceRegisterAsync(InvoiceRegister register, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteInvoiceRegisterAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> GetInvoiceRegistersAsync(int page, int size, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> SearchInvoiceRegistersAsync(int? id,
        string? registerNumber,
        DateTime? arrivalDate,
        int? supplierId,
        int? productId,
        int? physicalWeightReg,
        int? shrinkageReg,
        int? wasteReg,
        int? accWeightReg,
        double? weedImpurityBase,
        double? moistureBase,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size,
        CancellationToken cancellationToken);
}