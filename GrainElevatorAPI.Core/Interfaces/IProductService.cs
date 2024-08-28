using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IProductService
{
    Task<Product> AddProductAsync(string  title);
    Task<Product> GetProductByIdAsync(int id);
    Task<Product> UpdateProductAsync(Product product);
    Task<bool> DeleteProductAsync(int id);
    IEnumerable<Product> GetProducts(int page, int size);
    IEnumerable<Product> SearchProduct(string title);
}