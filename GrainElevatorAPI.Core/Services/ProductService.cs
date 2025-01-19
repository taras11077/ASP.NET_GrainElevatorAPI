using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainElevatorAPI.Core.Services;

public class ProductService : IProductService
{
    private readonly IRepository _repository;

    public ProductService(IRepository repository)
    {
        _repository = repository;
    }
    
 public async Task<Product> CreateProductAsync(Product product, int createdById, CancellationToken cancellationToken)
    {
        try
        {
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedById = createdById;
            
            return await _repository.AddAsync(product, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Продукції", ex);
        }
    }
 
    public async Task<Product> GetProductByIdAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetByIdAsync<Product>(id, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Продукції з ID {id}", ex);
        }
    }

    
    public async Task<IEnumerable<Product>> GetProducts(int page, int size, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Product>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при отриманні списку Продукції", ex);
        }
    }
    public async Task<(IEnumerable<Product>, int)> SearchProductsAsync(
        string? title,
        string? createdByName,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken)
    {
        try
        {
            
            var query = _repository.GetAll<Product>()
                .Include(s => s.CreatedBy)
                .Where(to => to.RemovedAt == null);
            
            // Виклик методу фільтрації
            query = ApplyFilters(query, title, createdByName);

            // Виклик методу сортування
            query = ApplySorting(query, sortField, sortOrder);

            // Пагінація
            int totalCount = await query.CountAsync(cancellationToken);

            var filteredProducts = await query
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return (filteredProducts, totalCount);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Продукції", ex);
        }
    }
    
    private IQueryable<Product> ApplyFilters(
        IQueryable<Product> query,
        string? title,
        string? createdByName)
    {
        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title == title);
        }
        if (!string.IsNullOrEmpty(createdByName))
        {
            query = query.Where(p => p.CreatedBy.LastName == createdByName);
        }
        
        return query;
    }
    
    private IQueryable<Product> ApplySorting(
        IQueryable<Product> query,
        string? sortField,
        string? sortOrder)
    {
        if (string.IsNullOrEmpty(sortField)) return query; // Без сортування

        return sortField switch
        {
            "title" => sortOrder == "asc"
                ? query.OrderBy(p => p.Title)
                : query.OrderByDescending(p => p.Title),
            "createdByName" => sortOrder == "asc"
                ? query.OrderBy(p => p.CreatedBy.LastName)
                : query.OrderByDescending(p => p.CreatedBy.LastName),
            _ => query // Якщо поле не визначене
        };
    }
    
    
    public async Task<Product> UpdateProductAsync(Product product, int modifiedById, CancellationToken cancellationToken)
    {
        try
        {
            product.ModifiedAt = DateTime.UtcNow;
            product.ModifiedById = modifiedById;
            
            return await _repository.UpdateAsync(product, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Продукції з ID  {product.Id}", ex);
        }
    }
    
    public async Task<Product> SoftDeleteProductAsync(Product product, int removedById, CancellationToken cancellationToken)
    {
        try
        {
            product.RemovedAt = DateTime.UtcNow;
            product.RemovedById = removedById;
            
            return await _repository.UpdateAsync(product, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Продукції з ID  {product.Id}", ex);
        }
    }
    
    public async Task<Product> RestoreRemovedProductAsync(Product product, int restoredById, CancellationToken cancellationToken)
    {
        try
        {
            product.RemovedAt = null;
            product.RestoredAt = DateTime.UtcNow;
            product.RestoreById = restoredById;
            
            return await _repository.UpdateAsync(product, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Продукції з ID  {product.Id}", ex);
        }
    }

    public async Task<bool> DeleteProductAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            var product = await _repository.GetByIdAsync<Product>(id, cancellationToken);
            if (product != null)
            {
                await _repository.DeleteAsync<Product>(id, cancellationToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Продукції з ID {id}", ex);
        }
    }
}