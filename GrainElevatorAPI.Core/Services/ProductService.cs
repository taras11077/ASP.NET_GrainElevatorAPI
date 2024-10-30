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
    public async Task<IEnumerable<Product>> SearchProduct(string title, CancellationToken cancellationToken)
    {
        try
        {
            return await _repository.GetAll<Product>()
                .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Продукції з назвою {title}", ex);
        }
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