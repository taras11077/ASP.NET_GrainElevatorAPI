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
    
 public async Task<Product> CreateProductAsync(Product product, int createdById)
    {
        try
        {
            product.CreatedAt = DateTime.UtcNow;
            product.CreatedById = createdById;
            
            await _repository.AddAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка сервісу при додаванні Продукції", ex);
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
            throw new Exception("Помилка сервісу при отриманні списку Продукції", ex);
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            return await _repository.GetByIdAsync<Product>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при отриманні Продукції з ID {id}", ex);
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
            throw new Exception($"Помилка сервісу при отриманні Продукції з назвою {title}", ex);
        }
    }
    
    public async Task<Product> UpdateProductAsync(Product product, int modifiedById)
    {
        try
        {
            product.ModifiedAt = DateTime.UtcNow;
            product.ModifiedById = modifiedById;
            
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при оновленні Продукції з ID  {product.Id}", ex);
        }
    }
    
    public async Task<Product> SoftDeleteProductAsync(Product product, int removedById)
    {
        try
        {
            product.RemovedAt = DateTime.UtcNow;
            product.RemovedById = removedById;
            
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при видаленні Продукції з ID  {product.Id}", ex);
        }
    }
    
    public async Task<Product> RestoreRemovedProductAsync(Product product, int restoredById)
    {
        try
        {
            product.RemovedAt = null;
            product.RestoredAt = DateTime.UtcNow;
            product.RestoreById = restoredById;
            
            await _repository.UpdateAsync(product);
            await _repository.SaveChangesAsync();

            return product;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при відновленні Продукції з ID  {product.Id}", ex);
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var product = await _repository.GetByIdAsync<Product>(id);
            if (product != null)
            {
                await _repository.DeleteAsync<Product>(id);
                await _repository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка сервісу при hard-видаленні Продукції з ID {id}", ex);
        }
    }
}