using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IWarehouseUnitService
{
    Task<WarehouseUnit> WarehouseTransferAsync(int invoiceRegisterId, int createdById);
    Task<WarehouseUnit> GetWarehouseUnitByIdAsync(int id);
    Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit unit, int modifiedById);
    Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById);
    Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById);
    Task<bool> DeleteWarehouseUnitAsync(int id);
    IQueryable<WarehouseUnit> GetPagedWarehouseUnits(int page, int size);
    IEnumerable<WarehouseUnit> SearchWarehouseUnits(int? id,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size);
}