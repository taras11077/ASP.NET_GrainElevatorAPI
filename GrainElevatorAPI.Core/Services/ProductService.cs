using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class ProductService : IProductService
{
    private readonly IRepository _repository;

    public ProductService(IRepository repository)
    {
        _repository = repository;
    }
    
 public async Task<Product> AddProductAsync(string title)
    {
        try
        {
            var newProductTitle = new Product{ Title = title };
            
            return await _repository.Add(newProductTitle);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Назви продукту", ex);
        }
    }

    public async Task<Product> GetProductByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<Product>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Назви продукту з ID {id}", ex);
        }
    }

    public async Task<Product> UpdateProductAsync(Product product)
    {
        try
        {
            return await _repository.Update(product);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Назви продукту з ID  {product.Id}", ex);
        }
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var productTitle = await _repository.GetById<Product>(id);
            if (productTitle != null)
            {
                await _repository.Delete<Product>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Назви продукту з ID {id}", ex);
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

    public IEnumerable<Product> SearchProduct(string title)
    {
        try
        {
            return _repository.GetAll<Product>()
                .Where(r => r.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Назви продукту з назвою {title}", ex);
        }
    }
}