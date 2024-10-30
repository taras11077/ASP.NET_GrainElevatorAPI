using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;
using GrainElevatorAPI.Core.Models;

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
            throw new Exception("Помилка при додаванні Назви продукту", ex);
        }
    }
 
    public IEnumerable<Product> GetProducts(int page, int size)
    {
        try
        {
            return _repository.GetAll<Product>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Назв продукту", ex);
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
            throw new Exception($"Помилка при отриманні Назви продукту з ID {id}", ex);
        }
    }

    public IEnumerable<Product> SearchProduct(string title)
    {
        try
        {
            return _repository.GetAll<Product>()
                .Where(p => p.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Назви продукту з назвою {title}", ex);
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
            throw new Exception($"Помилка при оновленні Назви продукту з ID  {product.Id}", ex);
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
            throw new Exception($"Помилка при видаленні Продукції з ID  {product.Id}", ex);
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
            throw new Exception($"Помилка при відновленні Продукції з ID  {product.Id}", ex);
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
            throw new Exception($"Помилка при видаленні Назви продукту з ID {id}", ex);
        }
    }
}