using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class ProductTitleService : IProductTitleService
{
    private readonly IRepository _repository;

    public ProductTitleService(IRepository repository)
    {
        _repository = repository;
    }
    
 public async Task<ProductTitle> AddProductTitleAsync(string title)
    {
        try
        {
            var newProductTitle = new ProductTitle{ Title = title };
            
            return await _repository.Add(newProductTitle);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Назви продукту", ex);
        }
    }

    public async Task<ProductTitle> GetProductTitleByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<ProductTitle>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Назви продукту з ID {id}", ex);
        }
    }

    public async Task<ProductTitle> UpdateProductTitleAsync(ProductTitle productTitle)
    {
        try
        {
            return await _repository.Update(productTitle);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Назви продукту з ID  {productTitle.Id}", ex);
        }
    }

    public async Task<bool> DeleteProductTitleAsync(int id)
    {
        try
        {
            var productTitle = await _repository.GetById<ProductTitle>(id);
            if (productTitle != null)
            {
                await _repository.Delete<ProductTitle>(id);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Назви продукту з ID {id}", ex);
        }
    }

    public IEnumerable<ProductTitle> GetProductTitles(int page, int size)
    {
        try
        {
            return _repository.GetAll<ProductTitle>()
                .Skip((page - 1) * size)
                .Take(size)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Назв продукту", ex);
        }
    }

    public IEnumerable<ProductTitle> SearchProductTitle(string title)
    {
        try
        {
            return _repository.GetAll<ProductTitle>()
                .Where(r => r.Title.ToLower().Contains(title.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Назви продукту з назвою {title}", ex);
        }
    }
}