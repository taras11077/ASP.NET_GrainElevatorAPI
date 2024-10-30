using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseProductCategoryService : IWarehouseProductCategoryService
{
     private readonly IRepository _repository;

    public WarehouseProductCategoryService(IRepository repository) => _repository = repository;


    public async Task<WarehouseProductCategory> CreateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseProductCategory.CreatedAt = DateTime.UtcNow;
            warehouseProductCategory.CreatedById = createdById;
            
            return await _repository.AddAsync(warehouseProductCategory, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Категорії продукції", ex);
        }
    }
    public async Task<IEnumerable<WarehouseProductCategory>> GetWarehouseProductCategories(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<WarehouseProductCategory>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Категорій продукції", ex);
        }
    }
    
    
    public async Task<WarehouseProductCategory> GetWarehouseProductCategoryByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<WarehouseProductCategory>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Категорії продукції з ID {id}", ex);
        }
    }

     public async Task<IEnumerable<WarehouseProductCategory>> SearchWarehouseProductCategories(int? id,
         string? title,
         int? supplierId,
         int? productId,
         int? createdById,
         DateTime? removedAt,
         int page,
         int size, 
         CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetAll<WarehouseProductCategory>()
                .Include(wpc => wpc.WarehouseUnit)
                .AsQueryable();

            if (id.HasValue)
            {
                query = query.Where(wpc => wpc.Id == id);
            }
            
            if (!string.IsNullOrEmpty(title))
            {
                query = query.Where(wpc => wpc.Title == title);
            }
            
            if (supplierId.HasValue)
                query = query.Where(wpc => wpc.WarehouseUnit != null && wpc.WarehouseUnit.SupplierId == supplierId.Value);
            
            if (productId.HasValue)
                query = query.Where(wpc => wpc.WarehouseUnit != null && wpc.WarehouseUnit.ProductId == productId.Value);
            
            if (createdById.HasValue)
                query = query.Where(wpc => wpc.CreatedById == createdById.Value);

            if (removedAt.HasValue)
                query = query.Where(wpc => wpc.RemovedAt == removedAt.Value);
            
            return await query.ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Категорій продукції", ex);
        }
    }
    
    public async Task<WarehouseProductCategory> UpdateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseProductCategory.ModifiedAt = DateTime.UtcNow;
            warehouseProductCategory.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(warehouseProductCategory, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }

    public async Task<WarehouseProductCategory> SoftDeleteWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseProductCategory.RemovedAt = DateTime.UtcNow;
            warehouseProductCategory.RemovedById = removedById;
            
            return await _repository.UpdateAsync(warehouseProductCategory, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }
    
    public async Task<WarehouseProductCategory> RestoreRemovedWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            warehouseProductCategory.RemovedAt = null;
            warehouseProductCategory.RestoredAt = DateTime.UtcNow;
            warehouseProductCategory.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(warehouseProductCategory, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteWarehouseProductCategoryAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var warehouseProductCategory = await _repository.GetByIdAsync<WarehouseProductCategory>(id, cancellationToken);
            if (warehouseProductCategory != null)
            {
                await _repository.DeleteAsync<WarehouseProductCategory>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Категорії продукції з ID {id}", ex);
        }
    }
}