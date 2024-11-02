using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IInvoiceRegisterService
{
    Task<InvoiceRegister> CreateRegisterAsync(
        string registerNumber,
        int supplierId,
        int productId,
        DateTime arrivalDate,
        double weedImpurityBase,
        double moistureBase,
        IEnumerable<int> laboratoryCardIds,
        int createdById,
        CancellationToken cancellationToken);
    Task<InvoiceRegister> GetRegisterByIdAsync(int id, CancellationToken cancellationToken);
    Task<InvoiceRegister> UpdateRegisterAsync(InvoiceRegister register, CancellationToken cancellationToken);
    Task<InvoiceRegister> SoftDeleteRegisterAsync(InvoiceRegister register, int removedById, CancellationToken cancellationToken);
    Task<InvoiceRegister> RestoreRemovedRegisterAsync(InvoiceRegister register, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteRegisterAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> GetRegistersAsync(int page, int size, CancellationToken cancellationToken);
    Task<IEnumerable<InvoiceRegister>> SearchRegistersAsync(int? id,
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