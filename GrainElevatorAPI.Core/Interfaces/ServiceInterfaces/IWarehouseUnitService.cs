using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IWarehouseUnitService
{
    Task<WarehouseUnit> WarehouseTransferAsync(InvoiceRegister register, int createdById, CancellationToken cancellationToken);
    Task<WarehouseUnit> GetWarehouseUnitByIdAsync(int id, CancellationToken cancellationToken);
    Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit unit, int modifiedById, CancellationToken cancellationToken);
    Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById, CancellationToken cancellationToken);
    Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteWarehouseUnitAsync(int id, CancellationToken cancellationToken);
    IQueryable<WarehouseUnit> GetPagedWarehouseUnits(int page, int size);
    IEnumerable<WarehouseUnit> SearchWarehouseUnits(int? id,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size);
}