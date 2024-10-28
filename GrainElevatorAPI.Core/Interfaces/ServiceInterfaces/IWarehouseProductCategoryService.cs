using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface IWarehouseProductCategoryService
{
    Task<WarehouseProductCategory> CreateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int createdById);
    Task<WarehouseProductCategory> GetWarehouseProductCategoryByIdAsync(int id);
    Task<WarehouseProductCategory> UpdateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int modifiedById);
    Task<WarehouseProductCategory> SoftDeleteWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int removedById);
    Task<WarehouseProductCategory> RestoreRemovedWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int restoredById);
    Task<bool> DeleteWarehouseProductCategoryAsync(int id);
    IQueryable<WarehouseProductCategory> GetWarehouseProductCategories(int page, int size);
    IEnumerable<WarehouseProductCategory> SearchWarehouseProductCategories(int? id,
        string? title,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size);
}