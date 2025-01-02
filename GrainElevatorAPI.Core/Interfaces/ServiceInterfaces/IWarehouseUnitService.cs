using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IWarehouseUnitService
{
    Task<WarehouseUnit> CreateWarehouseUnitAsync(
        string supplierTitle,
        string productTitle,
        int employeeId,
        CancellationToken cancellationToken);
    
    Task<WarehouseUnit> WarehouseTransferAsync(InvoiceRegister register, int employeeId, CancellationToken cancellationToken);
    Task<WarehouseUnit> GetWarehouseUnitByIdAsync(int id, CancellationToken cancellationToken);

    Task DeletingRegisterDataFromWarehouseUnitAsync(InvoiceRegister register, int modifiedById,
        CancellationToken cancellationToken);

    Task DeletingOutputInvoiceDataFromWarehouseUnitAsync(OutputInvoice invoice, int modifiedById,
        CancellationToken cancellationToken);
    Task<WarehouseUnit> UpdateWarehouseUnitAsync(WarehouseUnit unit, int modifiedById, CancellationToken cancellationToken);
    Task<WarehouseUnit> SoftDeleteWarehouseUnitAsync(WarehouseUnit warehouseUnit, int removedById, CancellationToken cancellationToken);
    Task<WarehouseUnit> RestoreRemovedWarehouseUnitAsync(WarehouseUnit warehouseUnit, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteWarehouseUnitAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<WarehouseUnit>> GetPagedWarehouseUnitsAsync(int page, int size, CancellationToken cancellationToken);
    Task<(IEnumerable<WarehouseUnit>, int)> SearchWarehouseUnitsAsync(
        string? supplierTitle,
        string? productTitle,
        string? createdByName,
        int page,
        int size, 
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
}