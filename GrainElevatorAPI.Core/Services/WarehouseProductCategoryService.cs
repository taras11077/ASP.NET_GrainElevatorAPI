using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class WarehouseProductCategoryService : IWarehouseProductCategoryService
{
     private readonly IRepository _repository;

    public WarehouseProductCategoryService(IRepository repository) => _repository = repository;


    public async Task<WarehouseProductCategory> CreateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int createdById)
    {
        try
        {
            warehouseProductCategory.CreatedAt = DateTime.UtcNow;
            warehouseProductCategory.CreatedById = createdById;
            
            await _repository.AddAsync(warehouseProductCategory);
            await _repository.SaveChangesAsync();
            
            return warehouseProductCategory;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Категорії продукції", ex);
        }
    }
    public IQueryable<WarehouseProductCategory> GetWarehouseProductCategories(int page, int size)
    {
        try
        {
            return _repository.GetAll<WarehouseProductCategory>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Категорій продукції", ex);
        }
    }
    
    
    public async Task<WarehouseProductCategory> GetWarehouseProductCategoryByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<WarehouseProductCategory>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Категорії продукції з ID {id}", ex);
        }
    }

     public IEnumerable<WarehouseProductCategory> SearchWarehouseProductCategories(int? id,
         string? title,
         int? supplierId,
         int? productId,
         int? createdById,
         DateTime? removedAt,
         int page,
         int size)
    {
        try
        {
            var query = GetWarehouseProductCategories(page, size)
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
            
            return query.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при пошуку Категорій продукції", ex);
        }
    }
    
    public async Task<WarehouseProductCategory> UpdateWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int modifiedById)
    {
        try
        {
            warehouseProductCategory.ModifiedAt = DateTime.UtcNow;
            warehouseProductCategory.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(warehouseProductCategory);
            await _repository.SaveChangesAsync();
            
            return warehouseProductCategory;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }

    public async Task<WarehouseProductCategory> SoftDeleteWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int removedById)
    {
        try
        {
            warehouseProductCategory.RemovedAt = DateTime.UtcNow;
            warehouseProductCategory.RemovedById = removedById;
            
            await _repository.UpdateAsync(warehouseProductCategory);
            await _repository.SaveChangesAsync();
            
            return warehouseProductCategory;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }
    
    public async Task<WarehouseProductCategory> RestoreRemovedWarehouseProductCategoryAsync(WarehouseProductCategory warehouseProductCategory, int restoredById)
    {
        try
        {
            warehouseProductCategory.RemovedAt = null;
            warehouseProductCategory.RestoredAt = DateTime.UtcNow;
            warehouseProductCategory.RestoreById = restoredById;
            
            await _repository.UpdateAsync(warehouseProductCategory);
            await _repository.SaveChangesAsync();
            
            return warehouseProductCategory;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Категорії продукції з ID  {warehouseProductCategory.Id}", ex);
        }
    }
    
    public async Task<bool> DeleteWarehouseProductCategoryAsync(int id)
    {
        try
        {
            var warehouseProductCategory = await _repository.GetByIdAsync<WarehouseProductCategory>(id);
            if (warehouseProductCategory != null)
            {
                await _repository.DeleteAsync<WarehouseProductCategory>(id);
                await _repository.SaveChangesAsync();
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