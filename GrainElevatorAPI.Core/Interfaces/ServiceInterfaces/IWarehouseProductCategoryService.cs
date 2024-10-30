using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IWarehouseProductCategoryService
{
    Task<WarehouseProductCategory> CreateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int createdById, CancellationToken cancellationToken);
    Task<WarehouseProductCategory> GetWarehouseProductCategoryByIdAsync(int id, CancellationToken cancellationToken);
    Task<WarehouseProductCategory> UpdateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int modifiedById, CancellationToken cancellationToken);
    Task<WarehouseProductCategory> SoftDeleteWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int removedById, CancellationToken cancellationToken);
    Task<WarehouseProductCategory> RestoreRemovedWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteWarehouseProductCategoryAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<WarehouseProductCategory>> GetWarehouseProductCategories(int page, int size, CancellationToken cancellationToken);
    Task<IEnumerable<WarehouseProductCategory>> SearchWarehouseProductCategories(int? id,
        string? title,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size, 
        CancellationToken cancellationToken);
}